package org.gasolineras.adicionalmovil;

import java.io.IOException;

import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpResponseException;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import android.os.AsyncTask;
import android.util.Log;

public class WS_ObtenerDatos extends AsyncTask<String, String, String> {

	private static final String NAME_SPACE = "http://adicional.gasolineras.org/";
	private MainActivity activity;

	public WS_ObtenerDatos(MainActivity activity) {
		this.activity = activity;
	}

	@Override
	protected String doInBackground(String... params) {
		String resultado = "";
		String error = "";

		try {
			// el request
			SoapObject request = new SoapObject(NAME_SPACE, params[1]);

			if (!params[1].equalsIgnoreCase("ObtenerEstadoFlujo")) {
				request.addProperty("std", params[3]);
				request.addProperty("numCelular", params[2]);
			} else {
				request.addProperty("numCelular", params[2]);
			}

			// el ensobretado
			SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(
					SoapEnvelope.VER11);
			envelope.dotNet = true;
			envelope.setOutputSoapObject(request);

			// el transporte
			HttpTransportSE transporte = new HttpTransportSE(params[0]);

			// se hace la llamada
			transporte.call(NAME_SPACE + params[1], envelope);

			// se obtiene el resultado
			// SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
			resultado = envelope.getResponse().toString();

			// SoapObject response = (SoapObject)r;
			// for(int i= 0; i < response.getPropertyCount(); i++){
			// resultado +=
			// ((SoapObject)response.getProperty(i)).getPropertyAsString(1);
			// }
		} catch (RuntimeException e) {
			e.printStackTrace();
			error = e.getLocalizedMessage();
		} catch (HttpResponseException e) {
			e.printStackTrace();
			error = e.getLocalizedMessage();
		} catch (IOException e) {
			e.printStackTrace();
			error = e.getLocalizedMessage();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
			error = e.getLocalizedMessage();
		} catch (Exception e) {
			e.printStackTrace();
			error = e.getLocalizedMessage() == null ? "Canal de comunicación no disponible"
					: e.getLocalizedMessage();

			StackTraceElement[] stack = e.getStackTrace();
			String Trace = "";

			for (StackTraceElement line : stack) {
				Trace += line.toString() + "\r\n";
			}

			Log.e("GC.Proveedor.call[Exception]", Trace.trim());
		}

		if (error.length() > 0) {
			/*
			 * publishProgress(error.contains("Permiso Denegado") ?
			 * "Permiso Denegado" : error);
			 */
			resultado = error.contains("Permiso Denegado") ? "Permiso Denegado"
					: error;
		}

		return resultado;
	}

	@Override
	protected void onPostExecute(String result) {
		super.onPostExecute(result);
		this.activity.mostrarEspera(false);

		if (result.contains("Permiso Denegado")) {
			this.activity.mostrarMensaje("Permiso Denegado");
			result = "";
		} else if (result.equalsIgnoreCase("No se aplicó el flujo")) {
			this.activity.mostrarMensaje(result);
			result = "";
		} else if (result.equalsIgnoreCase("Si")
				|| result.equalsIgnoreCase("No")
				|| result.toLowerCase().contains("ksoap")) {
			result = "";
		}
		if (result.length() > 0) {
			this.activity.mostrarMensaje(result);
			this.activity.ResultadoExitoso(result);
		}
	}

	@Override
	protected void onPreExecute() {
		super.onPreExecute();
		this.activity.mostrarEspera(true);
	}
}
