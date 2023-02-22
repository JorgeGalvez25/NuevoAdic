package org.gasolineras.adicionalmovil;

import java.io.IOException;

import org.gasolineras.adicionalmovil.Entidades.Moviles;
import org.gasolineras.adicionalmovil.Entidades.Permisos;
import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpResponseException;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import android.os.AsyncTask;
import android.util.Log;

public class WS_RegistarMovil extends AsyncTask<String, String, String> {

	private static final String NAME_SPACE = "http://adicional.gasolineras.org/";
	//private static final String METHOD_NAME = "RegistrarCelular";
	//private static final String SOAP_ACTION = "http://adicional.gasolineras.org/RegistrarCelular";

	private Registro activity;

	public WS_RegistarMovil(Registro activity) {
		this.activity = activity;
	}

	@Override
	protected String doInBackground(String... params) {
		String resultado = "";
		String error = "";
		String SoapAction = NAME_SPACE + params[1];

		try {
			// el request
			SoapObject request = new SoapObject(NAME_SPACE, params[1]);
			Log.i("GC.NAME_SPACE", NAME_SPACE);
			Log.i("GC.params[1]", params[1]);

			String strI = "";
			for (int i = 0; i < params.length; i++) {
				strI = Integer.toString(i);
				Log.i("GC.params[" + strI + "]", strI + " - " + params[i]);
			}

			Moviles reg = new Moviles();
			reg.setTelefono(params[2]);
			reg.setResponsable(params[3]);
			reg.setPassword(params[4]);

			request.addProperty("sesion", reg);

			/*
			 * request.addProperty("numCelular", params[1]);
			 * request.addProperty("Usuario", params[2]);
			 * request.addProperty("Password", params[3]);
			 */

			// el ensobretado
			SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(
					SoapEnvelope.VER11);
			envelope.dotNet = true;
			envelope.setOutputSoapObject(request);
			Log.i("GC.Request", request.toString());
			envelope.addMapping(NAME_SPACE,
					Permisos.PERMISOS_CLASS.getSimpleName(),
					Permisos.PERMISOS_CLASS);
			envelope.addMapping(NAME_SPACE,
					Moviles.MOVILES_CLASS.getSimpleName(),
					Moviles.MOVILES_CLASS);

			// el transporte
			HttpTransportSE transporte = new HttpTransportSE(params[0]);
			Log.i("GC.envelope", envelope.toString());

			// se hace la llamada
			transporte.call(SoapAction, envelope);

			Object r = envelope.getResponse();
			Log.i("GC.Response", r.toString());
			// se ontiene el resultado
			SoapPrimitive response = (SoapPrimitive) r;
			resultado = response.toString();
			Log.i("GC.resultado", resultado);
		} catch (HttpResponseException e) {
			e.printStackTrace();
			error = e.getMessage();
		} catch (IOException e) {
			e.printStackTrace();
			error = e.getMessage();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
			error = e.getMessage();
		} catch (Exception ex) {
			ex.printStackTrace();
			error = ex.getMessage();
		}

		if (error.length() > 0) {
			Log.e("GC.error", error);
			if (error.contains("Submódulo Android no tiene licencia")) {
				error = "Submódulo Android no tiene licencia";
			} else if (error.contains("Canal de comunicación no disponible")) {
				error = "Canal de comunicación no disponible";
			} else if (error.contains("Movil inválido")) {
				error = "Movil inválido";
			} else if (error.contains("Password inválido")) {
				error = "Password inválido";
			}

			publishProgress(error);
			resultado = "";
		}

		return resultado;
	}

	@Override
	protected void onPostExecute(String result) {
		Log.i("GC.PostExecute.Resutl", result);
		this.activity.mostrarEspera(false);

		if (result.length() > 0) {
			this.activity.mostrarMensaje(result);
			this.activity.ResultadoExitoso(result);
		}
	}

	@Override
	protected void onPreExecute() {
		this.activity.mostrarEspera(true);
	}

	@Override
	protected void onProgressUpdate(String... params) {
		activity.mostrarMensaje(params[0]);
	}

}
