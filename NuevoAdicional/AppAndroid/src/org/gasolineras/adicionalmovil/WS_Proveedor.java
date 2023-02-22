package org.gasolineras.adicionalmovil;

import java.io.IOException;
import java.util.concurrent.ExecutionException;

import org.gasolineras.adicionalmovil.Entidades.Moviles;
import org.gasolineras.adicionalmovil.Entidades.Permisos;
import org.gasolineras.adicionalmovil.Entidades.Respuesta;
import org.gasolineras.adicionalmovil.utils.Proveedor_Callback;
import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import android.app.ProgressDialog;
import android.os.AsyncTask;
import android.os.Handler;
import android.os.Message;
import android.support.v7.app.ActionBarActivity;
import android.util.Log;
import android.widget.Toast;

public class WS_Proveedor {

	private class AsyncProveedor extends AsyncTask<Object, String, Object> {

		public String URL = "";
		private boolean isResultVector = false;
		private ProgressDialog dialog;

		private ActionBarActivity activity;
		private static final String NAME_SPACE = "http://adicional.gasolineras.org/";

		public String ResultadoObtenerEstadoFlujo = "";
		public String ResultadoRegistrarCelular = "";
		public String ResultadoSubirBajarFlujo = "";
		public String ResultadoValidarCelularSesion = "";
		public String ResultadoValidarCelular = "";
		public Moviles ResultadoObtenerMovil = new Moviles();
		private String ExceptionString = "";

		public AsyncProveedor(final ActionBarActivity activity) {
			this.activity = activity;
			dialog = new ProgressDialog(this.activity);
			dialog.setProgressStyle(ProgressDialog.STYLE_SPINNER);
			dialog.setMessage("Conectando al Servicio...");
			dialog.setCancelable(true);
			dialog.setMax(100);
		}

		private synchronized Moviles AsyncObtenerCelular(Moviles movil) {
			final String sGetMethod = "ObtenerCelular";
			final String svalue = "sesion";

			// Create the outgoing message
			SoapObject requestObject = new SoapObject(NAME_SPACE, sGetMethod);

			// Set Parameter type String
			requestObject.addProperty(svalue, movil);

			// Create soap envelop .use version 1.1 of soap
			SoapSerializationEnvelope envelope = GetEnvelope(requestObject);

			// call and Parse Result.
			Object response = this.call(NAME_SPACE + sGetMethod, envelope);
			Moviles result = null;
			if (response != null) {
				try {
					Respuesta resp = GetResponse(response);

					if (!resp.getIsFaulted()) {
						SoapObject obj = (SoapObject) resp.getResult();
						if (result == null) {
							result = new Moviles();
						}
						for (int i = 0; i < obj.getPropertyCount(); i++) {
							result.setProperty(i, obj.getProperty(i));
							Log.i("Loop-Result", obj.getProperty(i).toString());
						}
					}
				} catch (Exception e) {
					result = null;
					ExceptionString = GetErrorMessage(e.getMessage());
					Log.e("GC.Proveedor.ObtenerCelular", e.getMessage());
					e.printStackTrace();
				}
			}

			return result;
		}

		private synchronized String AsyncObtenerEstadoFlujo(String movil) {
			final String svalue = "numCelular";
			final String sGetMethod = "ObtenerEstadoFlujo";

			// Create the outgoing message
			SoapObject requestObject = new SoapObject(NAME_SPACE, sGetMethod);
			// Set Parameter type String
			requestObject.addProperty(svalue, movil);
			// Create soap envelop .use version 1.1 of soap
			SoapSerializationEnvelope envelope = GetEnvelope(requestObject);

			// call and Parse Result.
			final Object response = this
					.call(NAME_SPACE + sGetMethod, envelope);
			String result = null;

			if (response != null) {
				try {
					Respuesta resp = GetResponse(response);

					if (!resp.getIsFaulted()) {
						result = resp.getResult() == null ? "" : resp
								.getResult().toString();
					}
				} catch (Exception e) {
					result = null;
					ExceptionString = GetErrorMessage(e.getMessage());
					Log.e("GC.Proveedor.ObtenerEstadoFlujo",
							e.getMessage() == null ? "Excepcion" : e
									.getMessage());
					e.printStackTrace();
				}
			}

			return result;
		}

		private synchronized String AsyncRegistrarCelular(Moviles movil) {
			final String sGetMethod = "RegistrarCelular";
			final String svalue = "sesion";

			// Create the outgoing message
			SoapObject requestObject = new SoapObject(NAME_SPACE, sGetMethod);

			// Set Parameter type String
			requestObject.addProperty(svalue, movil);

			// Create soap envelop .use version 1.1 of soap
			SoapSerializationEnvelope envelope = GetEnvelope(requestObject);

			// call and Parse Result.
			Object response = this.call(NAME_SPACE + sGetMethod, envelope);
			String result = null;
			if (response != null) {
				try {
					Respuesta resp = GetResponse(response);

					if (!resp.getIsFaulted()) {
						result = resp.getResult() == null ? "" : resp
								.getResult().toString();
					}
				} catch (Exception e) {
					result = null;
					ExceptionString = GetErrorMessage(e.getMessage());
					Log.e("GC.Proveedor.RegistrarCelular", e.getMessage());
					e.printStackTrace();
				}
			}

			return result;
		}

		private synchronized String AsyncActualizarCelular(Moviles movil) {
			final String sGetMethod = "ActualizarCelular";
			final String svalue = "sesion";

			// Create the outgoing message
			SoapObject requestObject = new SoapObject(NAME_SPACE, sGetMethod);

			// Set Parameter type String
			requestObject.addProperty(svalue, movil);

			// Create soap envelop .use version 1.1 of soap
			SoapSerializationEnvelope envelope = GetEnvelope(requestObject);

			// call and Parse Result.
			Object response = this.call(NAME_SPACE + sGetMethod, envelope);
			String result = null;
			if (response != null) {
				try {
					Respuesta resp = GetResponse(response);

					if (!resp.getIsFaulted()) {
						result = resp.getResult() == null ? "" : resp
								.getResult().toString();
					}
				} catch (Exception e) {
					result = null;
					ExceptionString = GetErrorMessage(e.getMessage());
					Log.e("GC.Proveedor.ActualizarCelular", e.getMessage());
					e.printStackTrace();
				}
			}

			return result;
		}
		
		private synchronized String AsyncSubirBajarFlujo(String std,
				String numCelular) {
			final String svalue1 = "std";
			final String svalue2 = "numCelular";
			final String sGetMethod = "SubirBajarFlujo";

			// Create the outgoing message
			SoapObject requestObject = new SoapObject(NAME_SPACE, sGetMethod);

			// Set Parameter type String
			requestObject.addProperty(svalue1, std);
			requestObject.addProperty(svalue2, numCelular);

			// Create soap envelop .use version 1.1 of soap
			SoapSerializationEnvelope envelope = GetEnvelope(requestObject);

			// call and Parse Result.
			Object response = this.call(NAME_SPACE + sGetMethod, envelope);
			String result = null;

			if (response != null) {
				try {
					Respuesta resp = GetResponse(response);

					if (!resp.getIsFaulted()) {
						result = resp.getResult() == null ? "" : resp
								.getResult().toString();
					}
				} catch (Exception e) {
					result = null;
					ExceptionString = GetErrorMessage(e.getMessage());
					Log.e("GC.Proveedor.SubirBajarFlujo", e.getMessage());
					e.printStackTrace();
				}
			}

			return result;
		}

		private synchronized String AsyncValidarCelular(Moviles sesion) {
			final String sGetMethod = "ValidarCelularSesion";
			final String svalue = "sesion";

			// Create the outgoing message
			SoapObject requestObject = new SoapObject(NAME_SPACE, sGetMethod);

			// Set Parameter type String
			requestObject.addProperty(svalue, sesion);

			// Create soap envelop .use version 1.1 of soap
			SoapSerializationEnvelope envelope = GetEnvelope(requestObject);

			// call and Parse Result.
			Object response = this.call(NAME_SPACE + sGetMethod, envelope);
			String result = "";
			if (response != null) {
				try {
					Respuesta resp = GetResponse(response);

					if (!resp.getIsFaulted()) {
						result = resp.getResult() == null ? "" : resp
								.getResult().toString();
					}
				} catch (Exception e) {
					result = null;
					ExceptionString = GetErrorMessage(e.getMessage());
					Log.e("GC.Proveedor.ValidarCelular(Moviles)",
							e.getMessage());
					e.printStackTrace();
				}
			}
			return result;
		}

		private synchronized String AsyncValidarCelular(String telefono) {
			String result = null;

			try {
				final String sGetMethod = "ValidarCelular";

				// Create the outgoing message
				SoapObject requestObject = new SoapObject(NAME_SPACE,
						sGetMethod);
				requestObject.addProperty("telefono", telefono);

				// Create soap envelop .use version 1.1 of soap
				SoapSerializationEnvelope envelope = GetEnvelope(requestObject);
				// call and Parse Result.
				Object response = this.call(NAME_SPACE + sGetMethod, envelope);

				if (response != null) {
					try {
						Respuesta resp = GetResponse(response);

						if (!resp.getIsFaulted()) {
							result = resp.getResult() == null ? "" : resp
									.getResult().toString();
						}
					} catch (Exception e) {
						result = null;
						ExceptionString = GetErrorMessage(e.getMessage());
						Log.e("GC.Proveedor.ValidarCelular(String)",
								e.getMessage());
						e.printStackTrace();
					}
				}
			} catch (Exception e) {
				result = e.getLocalizedMessage() == null ? "" : e
						.getLocalizedMessage();
				Log.e("GC.Proveedor.ValidarCelular(String)", e.getMessage());
				StackTraceElement[] stack = e.getStackTrace();
				String Trace = "";
				for (StackTraceElement line : stack) {
					Trace += line.toString() + "\r\n";
				}

				Log.e("GC.Proveedor.ValidarCelular(String)", Trace);
			}

			return result;
		}

		protected Object call(String soapAction,
				SoapSerializationEnvelope envelope) {
			Object result = new Respuesta();
			Log.i("GC.Proveedor.call", URL);

			HttpTransportSE transportSE = new HttpTransportSE(URL);

			// call and Parse Result.
			try {
				Log.i("GC.Proveedor.call.soapAction", soapAction);
				transportSE.call(soapAction, envelope);

				if (!isResultVector) {
					result = envelope.getResponse();
					/*
					 * } else { Log.i("GC.Proveedor.call.soapAction.BodyInType",
					 * envelope.bodyIn.toString()); result = (Respuesta)
					 * envelope.bodyIn;
					 */
				}
			} catch (final IOException e) {
				result = e.getLocalizedMessage();
				StackTraceElement[] stack = e.getStackTrace();
				String Trace = "";

				for (StackTraceElement line : stack) {
					Trace += line.toString() + "\r\n";
				}

				Log.e("GC.Proveedor.call[IOException]", Trace.trim());
				e.printStackTrace();
			} catch (final XmlPullParserException e) {
				result = e.getLocalizedMessage();
				StackTraceElement[] stack = e.getStackTrace();
				String Trace = "";

				for (StackTraceElement line : stack) {
					Trace += line.toString() + "\r\n";
				}

				Log.e("GC.Proveedor.call[XmlPullParserException]", Trace.trim());
				e.printStackTrace();
			} catch (final Exception e) {
				result = e.getLocalizedMessage();
				StackTraceElement[] stack = e.getStackTrace();
				String Trace = "";

				for (StackTraceElement line : stack) {
					Trace += line.toString() + "\r\n";
				}

				Log.e("GC.Proveedor.call[Exception]", Trace.trim());
			}

			Log.i("GC.Proveedor.call.Result.Result", result == null ? "nulo"
					: result.toString());

			return result;
		}

		@Override
		protected Object doInBackground(Object... params) {
			Object result = null;
			try {
				if (params[0].toString().equalsIgnoreCase("ObtenerEstadoFlujo")) {
					result = AsyncObtenerEstadoFlujo(params[1].toString());
					ResultadoObtenerEstadoFlujo = GetErrorMessage(result
							.toString());
				} else if (params[0].toString().equalsIgnoreCase(
						"RegistrarCelular")) {
					result = AsyncRegistrarCelular((Moviles) params[1]);
				}  else if (params[0].toString().equalsIgnoreCase(
						"ActualizarCelular")) {
					result = AsyncActualizarCelular((Moviles) params[1]);
				} else if (params[0].toString().equalsIgnoreCase(
						"SubirBajarFlujo")) {
					result = AsyncSubirBajarFlujo(params[1].toString(),
							params[2].toString());
					ResultadoSubirBajarFlujo = GetErrorMessage(result
							.toString());
				} else if (params[0].toString().equalsIgnoreCase(
						"ValidarCelular1")) {
					result = AsyncValidarCelular((Moviles) params[1]);
					ResultadoValidarCelularSesion = GetErrorMessage(result
							.toString());
				} else if (params[0].toString().equalsIgnoreCase(
						"ValidarCelular2")) {
					result = AsyncValidarCelular(params[1].toString());
					ResultadoValidarCelular = GetErrorMessage(result.toString());
				} else if (params[0].toString().equalsIgnoreCase(
						"ObtenerCelular")) {
					result = AsyncObtenerCelular((Moviles) params[1]);
					ResultadoObtenerMovil = (result != null) ? null
							: (Moviles) result;
				}
			} catch (Exception e) {
				StackTraceElement[] stack = e.getStackTrace();
				String Trace = "";
				Log.e("GC.Proveedor.doBackground(String...).Exception.getLocalizedMessage",
						e.getLocalizedMessage() == null ? "" : e
								.getLocalizedMessage());
				for (StackTraceElement line : stack) {
					Trace += line.toString() + "\r\n";
				}
				Log.e("GC.Proveedor.doBackground(String...).Exception.getStackTrace",
						Trace.trim());
				e.printStackTrace();
			}
			return result;
		}

		@Override
		protected void onPreExecute() {
			dialog.setProgress(0);
			dialog.show();
		}

		@Override
		protected void onPostExecute(Object result) {
			synchronized (this) {
				try {
					super.onPostExecute(result);
					dialog.setProgress(0);
					dialog.dismiss();
					String msj = result == null ? "" : result.toString();

					if (ExceptionString.length() > 0) {
						Toast.makeText(this.activity, ExceptionString,
								Toast.LENGTH_LONG).show();
						result = null;
					} else if ((msj.equalsIgnoreCase("No") || msj.toString()
							.equalsIgnoreCase("Si"))
							|| msj.toLowerCase().contains("ksoap")
							|| msj.toLowerCase().contains(
									"org.gasolineras.adicionalmovil")) {
						result = null;
					} else if (msj.equalsIgnoreCase("No se aplicó el flujo")) {
						Toast.makeText(this.activity, result.toString(),
								Toast.LENGTH_LONG).show();
						result = null;
					}

					if (result != null && result.toString().length() > 0) {
						Toast.makeText(this.activity, result.toString(),
								Toast.LENGTH_LONG).show();
					}
				} catch (final Exception ex) {
					ex.printStackTrace();
				} finally {
					this.notifyAll();
				}
			}
		}

		@Override
		protected void onProgressUpdate(String... params) {
			super.onProgressUpdate(params);
			Toast.makeText(this.activity, params[0], Toast.LENGTH_LONG).show();
		}

		private String GetErrorMessage(String msj) {
			if (msj == null) {
				return "";
			}
			if (msj.length() <= 0) {
				return "";
			}
			String result = "";

			if (msj.contains("Permiso Denegado")) {
				result = "Permiso Denegado";
			} else if (msj.equalsIgnoreCase("No se aplicó el flujo")) {
				result = "No se aplicó el flujo";
			} else if (msj.contains("Submódulo Android no tiene licencia")) {
				result = "Submódulo Android no tiene licencia";
			} else if (msj.contains("Canal de comunicación no disponible")) {
				result = "Canal de comunicación no disponible";
			} else if (msj.contains("Móvil inválido")) {
				result = "Móvil inválido";
			} else if (msj.contains("Password inválido")) {
				result = "Password inválido";
			} else if (msj.contains("No tiene permitido cambiar el flujo.")) {
				result = "No tiene permitido cambiar el flujo.";
			} else if (msj.contains("Móvil no existe")){
				result = "Móvil no existe";
			}
			return result;
		}

		private Respuesta GetResponse(Object response) {
			Respuesta resp = new Respuesta();
			if (response != null) {
				try {
					if (response != null
							&& response.getClass() == org.ksoap2.serialization.SoapObject.class) {
						SoapObject sObject = (SoapObject) response;
						for (int i = 0; i < sObject.getPropertyCount(); i++) {
							resp.setProperty(i, sObject.getProperty(i));
							Log.i("Loop-envelope", sObject.getProperty(i)
									.toString());
						}
					}

					if (resp.getIsFaulted()) {
						ExceptionString = resp.getMessage();
					}
				} catch (Exception e) {
					ExceptionString = GetErrorMessage(e.getMessage());
					Log.e("GC.Proveedor.ObtenerCelular", e.getMessage());
					e.printStackTrace();
				}
			}

			return resp;
		}

		private SoapSerializationEnvelope GetEnvelope(SoapObject requestObject) {
			SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(
					SoapEnvelope.VER11);
			envelope.dotNet = true;

			// add the outgoing object as the request
			envelope.setOutputSoapObject(requestObject);
			envelope.addMapping(NAME_SPACE,
					Permisos.PERMISOS_CLASS.getSimpleName(),
					Permisos.PERMISOS_CLASS);
			envelope.addMapping(NAME_SPACE,
					Moviles.MOVILES_CLASS.getSimpleName(),
					Moviles.MOVILES_CLASS);
			envelope.addMapping(NAME_SPACE,
					Respuesta.RESPUESTA_CLASS.getSimpleName(),
					Respuesta.RESPUESTA_CLASS);

			return envelope;
		}

	}

	public String URL = "";
	private ActionBarActivity activity;

	public WS_Proveedor(ActionBarActivity activity) {
		this.activity = activity;
	}

	public void BeginObtenerCelular(Moviles movil,
			final Proveedor_Callback callback) {
		if (this.URL.length() <= 0) {
			return;
		}
		AsyncProveedor servicio = new AsyncProveedor(this.activity) {
			@Override
			protected void onPostExecute(Object result) {
				super.onPostExecute(result);
				callback.Callback(result);
			}
		};
		servicio.URL = this.URL;
		servicio.execute(new Object[] { "ObtenerCelular", movil });
	}

	public void BeginObtenerEstadoFlujo(String movil,
			final Proveedor_Callback callback) {
		if (this.URL.length() <= 0) {
			return;
		}
		AsyncProveedor servicio = new AsyncProveedor(this.activity) {
			@Override
			protected void onPostExecute(Object result) {
				super.onPostExecute(result);
				callback.Callback(result);
			}
		};
		servicio.URL = this.URL;
		servicio.execute(new Object[] { "ObtenerEstadoFlujo", movil });
	}

	public void BeginRegistrarCelular(Moviles movil,
			final Proveedor_Callback callback) {
		if (this.URL.length() <= 0) {
			return;
		}
		AsyncProveedor servicio = new AsyncProveedor(this.activity) {
			@Override
			protected void onPostExecute(Object result) {
				super.onPostExecute(result);
				callback.Callback(result);
			}
		};
		servicio.URL = this.URL;
		servicio.execute(new Object[] { "RegistrarCelular", movil });
	}
	
	public void BeginActualizarCelular(Moviles movil,
			final Proveedor_Callback callback) {
		if (this.URL.length() <= 0) {
			return;
		}
		AsyncProveedor servicio = new AsyncProveedor(this.activity) {
			@Override
			protected void onPostExecute(Object result) {
				super.onPostExecute(result);
				callback.Callback(result);
			}
		};
		servicio.URL = this.URL;
		servicio.execute(new Object[] { "ActualizarCelular", movil });
	}

	public void BeginSubirBajarFlujo(String std, String numCelular,
			final Proveedor_Callback callback) {
		if (this.URL.length() <= 0) {
			return;
		}
		AsyncProveedor servicio = new AsyncProveedor(this.activity) {
			@Override
			protected void onPostExecute(Object result) {
				super.onPostExecute(result);
				callback.Callback(result);
			}
		};
		servicio.URL = this.URL;
		servicio.execute(new Object[] { "SubirBajarFlujo", std, numCelular });
	}

	public void BeginValidarCelular(Moviles sesion,
			final Proveedor_Callback callback) {
		if (this.URL.length() <= 0) {
			return;
		}
		AsyncProveedor servicio = new AsyncProveedor(this.activity) {
			@Override
			protected void onPostExecute(Object result) {
				super.onPostExecute(result);
				callback.Callback(result);
			}
		};
		servicio.URL = this.URL;
		servicio.execute(new Object[] { "ValidarCelular1", sesion });
	}

	public void BeginValidarCelular(String sesion,
			final Proveedor_Callback callback) {
		if (this.URL.length() <= 0) {
			return;
		}
		AsyncProveedor servicio = new AsyncProveedor(this.activity) {
			@Override
			protected void onPostExecute(Object result) {
				super.onPostExecute(result);
				callback.Callback(result);
			}
		};
		servicio.URL = this.URL;
		servicio.execute(new Object[] { "ValidarCelular2", sesion });
	}

	public Moviles ObtenerCelular(Moviles movil) {
		if (this.URL.length() <= 0) {
			return null;
		}
		Object result = null;

		try {
			AsyncProveedor servicio = new AsyncProveedor(this.activity);
			servicio.URL = this.URL;

			try {
				result = servicio.execute(
						new Object[] { "ObtenerCelular", movil }).get();
			} catch (InterruptedException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			} catch (ExecutionException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
		} catch (Exception ex) {
			Log.e("GC.Proveedor.ObtenerCelular.Excepcion",
					ex.getLocalizedMessage());
			ex.printStackTrace();
		}
		return (Moviles) result;
	}

	public String ObtenerEstadoFlujo(String movil) {
		if (this.URL.length() <= 0) {
			return "";
		}
		Object result = null;
		try {
			AsyncProveedor servicio = new AsyncProveedor(this.activity);
			servicio.URL = this.URL;
			try {
				result = servicio.execute(
						new Object[] { "ObtenerEstadoFlujo", movil }).get();
			} catch (InterruptedException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			} catch (ExecutionException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
		} catch (Exception ex) {
			Log.e("GC.Proveedor.ObtenerEstadoFlujo.Excepcion",
					ex.getLocalizedMessage());
			ex.printStackTrace();

		}
		return result == null ? "" : result.toString();
	}

	public String RegistrarCelular(Moviles movil) {
		if (this.URL.length() <= 0) {
			return "";
		}
		Object result = null;
		try {
			AsyncProveedor servicio = new AsyncProveedor(this.activity);
			servicio.URL = this.URL;
			try {
				result = servicio.execute(
						new Object[] { "RegistrarCelular", movil }).get();
			} catch (InterruptedException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			} catch (ExecutionException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
		} catch (Exception ex) {
			Log.e("GC.Proveedor.RegistrarCelular.Excepcion",
					ex.getLocalizedMessage());
			ex.printStackTrace();

		}
		return result == null ? "" : result.toString();
	}
	
	public String ActualizarCelular(Moviles movil) {
		if (this.URL.length() <= 0) {
			return "";
		}
		Object result = null;
		try {
			AsyncProveedor servicio = new AsyncProveedor(this.activity);
			servicio.URL = this.URL;
			try {
				result = servicio.execute(
						new Object[] { "ActualizarCelular", movil }).get();
			} catch (InterruptedException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			} catch (ExecutionException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
		} catch (Exception ex) {
			Log.e("GC.Proveedor.ActualizarCelular.Excepcion",
					ex.getLocalizedMessage());
			ex.printStackTrace();

		}
		return result == null ? "" : result.toString();
	}

	public String SubirBajarFlujo(String std, String numCelular) {
		if (this.URL.length() <= 0) {
			return "";
		}
		Object result = null;
		try {
			AsyncProveedor servicio = new AsyncProveedor(this.activity);
			servicio.URL = this.URL;
			try {
				result = servicio.execute(
						new Object[] { "SubirBajarFlujo", std, numCelular })
						.get();
			} catch (InterruptedException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			} catch (ExecutionException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
		} catch (Exception ex) {
			Log.e("GC.Proveedor.SubirBajarFlujo.Excepcion",
					ex.getLocalizedMessage());
			ex.printStackTrace();
		}
		return result == null ? "" : result.toString();
	}

	public String ValidarCelular(Moviles sesion) {
		if (this.URL.length() <= 0) {
			return "";
		}
		Object result = null;
		try {
			AsyncProveedor servicio = new AsyncProveedor(this.activity);
			servicio.URL = this.URL;
			try {
				result = servicio.execute(
						new Object[] { "ValidarCelular1", sesion }).get();
			} catch (InterruptedException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			} catch (ExecutionException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
		} catch (Exception ex) {
			Log.e("GC.Proveedor.ValidarCelularSesion.Excepcion",
					ex.getLocalizedMessage());
			ex.printStackTrace();
		}
		return result == null ? "" : result.toString();
	}

	public String ValidarCelular(String sesion) {
		if (this.URL.length() <= 0) {
			return "";
		}
		Object result = null;
		try {
			AsyncProveedor servicio = new AsyncProveedor(this.activity);
			servicio.URL = this.URL;
			try {
				result = servicio.execute(
						new Object[] { "ValidarCelular2", sesion }).get();
			} catch (InterruptedException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			} catch (ExecutionException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
		} catch (Exception ex) {
			Log.e("GC.Proveedor.ValidarCelular.Excepcion",
					ex.getLocalizedMessage());
			ex.printStackTrace();
		}
		return result == null ? "" : result.toString();
	}
}
