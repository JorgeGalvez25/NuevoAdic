package org.gasolineras.adicionalmovil;

import java.io.IOException;
import java.util.Timer;
import java.util.TimerTask;

import org.gasolineras.adicionalmovil.Entidades.Moviles;
import org.gasolineras.adicionalmovil.Entidades.Respuesta;
import org.gasolineras.adicionalmovil.utils.Proveedor_Callback;
import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpResponseException;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import android.app.ProgressDialog;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Resources;
import android.os.Bundle;
import android.os.Looper;
import android.support.v4.app.Fragment;
import android.support.v4.app.TaskStackBuilder;
import android.support.v7.app.ActionBarActivity;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

public class MainActivity extends ActionBarActivity {

	/**
	 * A placeholder fragment containing a simple view.
	 */
	public static class PlaceholderFragment extends Fragment {

		public PlaceholderFragment() {
		}

		@Override
		public View onCreateView(LayoutInflater inflater, ViewGroup container,
				Bundle savedInstanceState) {
			View rootView = inflater.inflate(R.layout.fragment_main, container,
					false);
			return rootView;
		}
	}

	// Variables para el Servicio
	private String cellNumber;
	private String nombreUsuario;
	private String passwordUsuario;
	private String urlServicio;

	private Timer timer;

	private String nombreEstacion;

	// Para el estado del flujo
	private String fluStd;

	private WS_Proveedor Proveedor;

	// Para mostrar mientras está contactando al servicio
	ProgressDialog dialog;
	// Controles
	private TextView lblNumeroCell;
	private TextView lblUsuario;
	private TextView lblEstacion;
	private ProgressBar pbar;
	private ImageView imgSemaforo;

	private Button btnFlujo;
	private Button btnClear;

	private TextView lblFlustd;
	private Boolean subirBajar = false;

	// LLamar la ventana de configuración
	public void btnConfiguracionOnClick(View v) {
		Registro.IsForLogin = false;
		Intent registro = new Intent(this, Registro.class);
		startActivityForResult(registro, 1);
	}

	// Bajar/Subir el flujo
	public void btnFlujoOnClick(View v) {
		this.Proveedor.BeginSubirBajarFlujo(
				fluStd.equals("Estandar") ? "Mínimo" : "Estandar",
				getValorSP("Celular"), new Proveedor_Callback() {
					@Override
					public void Callback(Object result) {
						if (result != null) {
							ResultadoExitoso(result.toString());
						}
					};
				});
		// ConectarAlServicio("SubirBajarFlujo");
	}

	// Actualizar el estado
	public void btnRefreshOnClick(View v) {
		if (this.Proveedor.URL.length() <= 0) {
			Resources res = getResources();
			this.Proveedor.URL = getValorSP("uri").length() <= 0 ? "" : res
					.getString(R.string.prefijoServicio)
					+ getValorSP("uri")
					+ res.getString(R.string.posfijoServicio);
		}
		this.Proveedor.BeginObtenerEstadoFlujo(this.getValorSP("Celular"),
				new Proveedor_Callback() {
					@Override
					public void Callback(Object result) {
						if (result != null) {
							ResultadoExitoso(result.toString());
						}
					};
				});
	}

	// Regresa el valor de una variable desde SharedPreferences
	private String getValorSP(String key) {
		Resources res = getResources();
		String nomSP = res.getString(R.string.nombreSharedPreferences);

		SharedPreferences preferencias = getSharedPreferences(nomSP,
				MODE_PRIVATE);
		String nombreUsuario = preferencias.getString(key, "");

		return nombreUsuario;
	}

	// Muestra el progressBar
	public void mostrarEspera(Boolean visible) {
		dialog.setProgress(0);

		if (visible) {
			dialog.show();
			// pbar.setVisibility(View.VISIBLE);
		} else {
			dialog.dismiss();
			// pbar.setVisibility(View.GONE);
		}
	}

	public void mostrarMensaje(String mensaje) {
		Toast.makeText(this, mensaje, Toast.LENGTH_LONG).show();
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		Resources res = getResources();

		if (requestCode == 1) {
			if (resultCode == RESULT_OK) {
				// Obtener el teléfono
				cellNumber = getValorSP("Celular");

				// Obtener el nombre del usuario
				nombreUsuario = getValorSP("Usuario");

				// Obtener la url del servicio
				urlServicio = res.getString(R.string.prefijoServicio)
						+ getValorSP("uri")
						+ res.getString(R.string.posfijoServicio);

				// Obtener el nombre de la estación
				nombreEstacion = getValorSP("Estacion");

				lblNumeroCell.setText(cellNumber);
				lblUsuario.setText(nombreUsuario);
				lblEstacion.setText(nombreEstacion);

				if (this.Proveedor.URL.length() <= 0) {
					this.Proveedor.URL = getValorSP("uri").length() <= 0 ? ""
							: res.getString(R.string.prefijoServicio)
									+ getValorSP("uri")
									+ res.getString(R.string.posfijoServicio);
				}

				btnRefreshOnClick(null);// ConectarAlServicio("ObtenerEstadoFlujo");

				Moviles movil = new Moviles();
				movil.setTelefono(cellNumber);
				btnFlujo.setEnabled(false);
				this.Proveedor.BeginObtenerCelular(movil,
						new Proveedor_Callback() {

							@Override
							public void Callback(Object result) {
								if (result != null) {
									Moviles movil = (Moviles) result;
									if (movil != null) {
										subirBajar = movil.getPermisos()
												.getSubirBajar();
										btnFlujo.setEnabled(subirBajar);
									}
								}
							}
						});
			} else {
				finish();
			}
		}
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {

		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);

		if (savedInstanceState == null) {
			getSupportFragmentManager().beginTransaction()
					.add(R.id.container, new PlaceholderFragment()).commit();
		}

		// Para obtener los recursos
		Resources res = getResources();

		// Obtener el número de Celular
		cellNumber = getValorSP("Celular");
		// Obtener el nombre del usuario
		nombreUsuario = getValorSP("Usuario");
		// Obtener el password del usuario
		passwordUsuario = getValorSP("Password");
		// Obtener la url del servicio
		urlServicio = (getValorSP("uri").length() <= 0) ? "" : res
				.getString(R.string.prefijoServicio)
				+ getValorSP("uri")
				+ res.getString(R.string.posfijoServicio);
		// Obtener el nombre de la estación
		nombreEstacion = getValorSP("Estacion");
		// Inicializar el flujo a estandar
		fluStd = "Estandar";

		// Inicializar los Controles
		lblNumeroCell = (TextView) findViewById(R.id.lblNumeroCelular);
		lblUsuario = (TextView) findViewById(R.id.lblNombreUsuario);
		lblEstacion = (TextView) findViewById(R.id.lblNombreEstacion);
		pbar = (ProgressBar) findViewById(R.id.pbarConectando);
		imgSemaforo = (ImageView) findViewById(R.id.imgSemaforo);
		btnFlujo = (Button) findViewById(R.id.btnFlujo);

		btnClear = (Button) findViewById(R.id.btnClear);
		lblFlustd = (TextView) findViewById(R.id.lblFlustd);

		btnFlujo.setEnabled(false);

		this.Proveedor = new WS_Proveedor(this);
		this.Proveedor.URL = urlServicio;

		Moviles movil = null;
		if (cellNumber.length() > 0) {
			if (this.Proveedor.URL.length() >= 0) {
				movil = new Moviles();
				movil.setTelefono(cellNumber);
				movil = this.Proveedor.ObtenerCelular(movil);

				if (movil != null) {
					subirBajar = movil.getPermisos().getSubirBajar();
					btnFlujo.setEnabled(subirBajar);
					Log.i("Permite cambio de Flujo",
							Boolean.toString(subirBajar));

					if (!nombreUsuario.equalsIgnoreCase(movil.getResponsable())) {
						nombreUsuario = "";
						passwordUsuario = "";
						setValorSP("", nombreUsuario);
						setValorSP("", passwordUsuario);
					} else if (!passwordUsuario.equalsIgnoreCase(movil
							.getPassword())
							|| movil.getPassword().length() <= 0) {
						passwordUsuario = "";
						setValorSP("", passwordUsuario);
					}
				}
			}
		}

		// Si no se ha dado de alta el numero
		// O si no esta registrado
		// O si el password NO es opcional
		// O no cuenta con una URL
		if ((cellNumber.length() <= 0)
				|| (nombreUsuario.length() <= 0 && passwordUsuario.length() <= 0)
				|| (nombreUsuario.length() > 0 && passwordUsuario.length() > 0)
				|| (urlServicio.length() <= 0)) {
			Registro.IsForLogin = cellNumber.length() > 0; // Si asignas
															// true, el
															// telefono
															// nunca se
															// registrará.
			Intent registro = new Intent(this, Registro.class);
			startActivityForResult(registro, 1);
		} else {
			onActivityResult(1, RESULT_OK, null);
		}

		this.Proveedor.URL = urlServicio;
		lblNumeroCell.setText(cellNumber);
		lblUsuario.setText(nombreUsuario);
		lblEstacion.setText(nombreEstacion);

		lblFlustd.setText("");

		pbar.setVisibility(View.GONE);

		// Listener de los botones
		btnClear.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				Resources res = getResources();
				String nomSP = res.getString(R.string.nombreSharedPreferences);

				SharedPreferences preferencias = getSharedPreferences(nomSP,
						MODE_PRIVATE);
				preferencias.edit().clear().commit();
			}
		});
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {

		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// Handle action bar item clicks here. The action bar will
		// automatically handle clicks on the Home/Up button, so long
		// as you specify a parent activity in AndroidManifest.xml.
		int id = item.getItemId();
		if (id == R.id.action_settings) {
			btnConfiguracionOnClick(null);
			return true;
		}
		return super.onOptionsItemSelected(item);
	}

	// Para probar el webservice (HolaMundo)
	public void PruebaSaludoOnClick(View v) {
		Thread hilo = new Thread() {
			String res;

			@Override
			public void run() {
				String NameSpace = getResources().getString(R.string.nameSpace);// "http://adicional.gasolineras.org";
				String URL = urlServicio;
				String MethodName = "HelloWorld";
				String SoapAction = "http://adicional.gasolineras.org/HelloWorld";

				SoapObject request = new SoapObject(NameSpace, MethodName);

				SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(
						SoapEnvelope.VER11);
				envelope.dotNet = true;
				envelope.addMapping(NameSpace,
						Respuesta.RESPUESTA_CLASS.getSimpleName(),
						Respuesta.RESPUESTA_CLASS);

				envelope.setOutputSoapObject(request);

				HttpTransportSE transporte = new HttpTransportSE(URL);

				try {
					transporte.call(SoapAction, envelope);
					SoapObject spResult = (SoapObject) envelope.getResponse();

					Respuesta soapResult = new Respuesta();
					for (int i = 0; i < spResult.getPropertyCount(); i++) {
						soapResult.setProperty(i, spResult.getProperty(i));
					}

					res = soapResult.getIsFaulted() ? soapResult.getMessage()
							: soapResult.getResult().toString();

				} catch (HttpResponseException e) {
					e.printStackTrace();
					res = e.getMessage();
				} catch (IOException e) {
					e.printStackTrace();
					res = e.getMessage();
				} catch (XmlPullParserException e) {
					e.printStackTrace();
					res = e.getMessage();
				}

				runOnUiThread(new Runnable() {

					@Override
					public void run() {
						Toast.makeText(MainActivity.this, res,
								Toast.LENGTH_LONG).show();
					}
				});
			}
		};

		hilo.start();

	}

	public void ResultadoErroneo(String resultado) {
		imgSemaforo.setImageResource(R.drawable.help2);
		fluStd = "Estandar";
		btnFlujo.setEnabled(false);
	}

	public void ResultadoExitoso(String resultado) {
		if (resultado.equalsIgnoreCase("Estandar")) {
			// Desaplicar = Bajar
			btnFlujo.setText("Desaplicar");
			imgSemaforo.setImageResource(R.drawable.bullet_ball_glass_green);
			fluStd = resultado;
			btnFlujo.setEnabled(subirBajar);
		} else if (resultado.equalsIgnoreCase("Mínimo")) {
			// Aplicar = Subir
			btnFlujo.setText("Aplicar");
			imgSemaforo.setImageResource(R.drawable.bullet_ball_glass_red);
			fluStd = resultado;
			btnFlujo.setEnabled(subirBajar);
		}
	}

	private void setValorSP(String key, String value) {
		Resources res = getResources();
		String nomSP = res.getString(R.string.nombreSharedPreferences);

		SharedPreferences preferencias = getSharedPreferences(nomSP,
				MODE_PRIVATE);
		SharedPreferences.Editor editor = preferencias.edit();

		editor.putString(key, value);
		editor.commit();
	}

}
