package org.gasolineras.adicionalmovil;

import org.gasolineras.adicionalmovil.Entidades.Moviles;
import org.gasolineras.adicionalmovil.utils.Proveedor_Callback;

import android.app.ProgressDialog;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Resources;
import android.content.res.Resources.NotFoundException;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.app.ActionBarActivity;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

public class Registro extends ActionBarActivity {

	/**
	 * A placeholder fragment containing a simple view.
	 */
	public static class PlaceholderFragment extends Fragment {

		public PlaceholderFragment() {
		}

		@Override
		public View onCreateView(LayoutInflater inflater, ViewGroup container,
				Bundle savedInstanceState) {
			View rootView = inflater.inflate(R.layout.fragment_registro,
					container, false);
			return rootView;
		}
	}

	public static boolean IsForLogin = false;
	// Variables
	private String cellNumber;
	private String nombreUsuario;
	private String urlServicio;

	private String passwordUsuario;

	public boolean registrado;

	private WS_Proveedor Proveedor;

	// Para mostrar mientras está contactando al servicio
	ProgressDialog dialog;
	// Controles
	private EditText txtNumCell;
	private EditText txtUsuario;
	private EditText txtUri;
	private EditText txtPassword;
	private Button btnRegresar;
	private Button btnRecord;
	private TextView lblRes;

	private ProgressBar pbarConectando;

	// Regresa el valor de una variable desde SharedPreferences
	private String getValorSP(String key) {
		Resources res = getResources();
		String nomSP = res.getString(R.string.nombreSharedPreferences);

		SharedPreferences preferencias = getSharedPreferences(nomSP,
				MODE_PRIVATE);
		String nombreUsuario = preferencias.getString(key, "");

		return nombreUsuario;
	}

	public void mostrarEspera(Boolean visible) {
		dialog.setProgress(0);

		if (visible) {
			dialog.show();
			// pbarConectando.setVisibility(View.VISIBLE);
		} else {
			dialog.dismiss();
			// pbarConectando.setVisibility(View.GONE);
		}
	}

	public void mostrarMensaje(String mensaje) {
		Toast.makeText(this, mensaje, Toast.LENGTH_LONG).show();
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_registro);

		if (savedInstanceState == null) {
			getSupportFragmentManager().beginTransaction()
					.add(R.id.container, new PlaceholderFragment()).commit();
		}

		this.Proveedor = new WS_Proveedor(this);
		// Obtener el número de Celular
		cellNumber = getValorSP("Celular");
		// Obtener el nombre del usuario
		nombreUsuario = getValorSP("Usuario");
		// Obtener el contraseña del usuario
		passwordUsuario = getValorSP("Password");
		// Obtener la url del servicio
		urlServicio = getValorSP("uri");

		registrado = false;

		// Inicializar los controles
		txtNumCell = (EditText) findViewById(R.id.txtCelular);
		txtUsuario = (EditText) findViewById(R.id.txtUsuario);
		txtPassword = (EditText) findViewById(R.id.txtPassword);
		txtUri = (EditText) findViewById(R.id.txtURL);
		btnRegresar = (Button) findViewById(R.id.btnRegresar);
		btnRecord = (Button) findViewById(R.id.btnRecord);
		lblRes = (TextView) findViewById(R.id.lblNombreEstacion);
		pbarConectando = (ProgressBar) findViewById(R.id.pbarConectando);

		// Presentar los valores
		txtNumCell.setText(cellNumber);
		txtUsuario.setText(nombreUsuario);

		if (!Registro.IsForLogin) {
			txtPassword.setText(passwordUsuario);
			this.setTitle(cellNumber.length() > 0 ? "Configuración" : "Registro");
			btnRegresar.setVisibility(cellNumber.length() > 0 ? View.VISIBLE: View.INVISIBLE);			
			btnRecord.setText(cellNumber.length() > 0 ? "Guardar" : "Registrar");
		} else {
			txtPassword.setText("");			
			btnRecord.setText("Aceptar");
			txtPassword.requestFocus();
			this.setTitle("Sesión");
			btnRegresar.setVisibility(View.INVISIBLE);
		}
		txtUri.setText(urlServicio);
		lblRes.setText("");
		pbarConectando.setVisibility(View.GONE);

		// Deshabilitar los controles si tienen datos
		if (cellNumber.length() > 0) {
			txtNumCell.setEnabled(false);
			// txtUsuario.setEnabled(false);			
			registrado = true;
		} else {
			txtNumCell.setFocusableInTouchMode(true);
			txtNumCell.requestFocus();			
		}

		if(btnRegresar.getVisibility() != View.INVISIBLE)
		{
			// EL listener del btnRegresar
			btnRegresar.setOnClickListener(new View.OnClickListener() {
	
				@Override
				public void onClick(View v) {
					Intent returnIntent = new Intent();
	
					if (registrado) {
						returnIntent.putExtra("CelRegistrado", true);
						setResult(RESULT_OK, returnIntent);
					} else {
						setResult(RESULT_CANCELED, returnIntent);
					}
	
					finish();
				}
			});
		}
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {

		// Inflate the menu; this adds items to the action bar if it is present.
		// getMenuInflater().inflate(R.menu.registro, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// Handle action bar item clicks here. The action bar will
		// automatically handle clicks on the Home/Up button, so long
		// as you specify a parent activity in AndroidManifest.xml.
		int id = item.getItemId();
		if (id == R.id.action_settings) {
			return true;
		}
		return super.onOptionsItemSelected(item);
	}

	// Registrar el Celular
	public void registrarMovilOnClick(View v) {
		if (txtNumCell.getText().toString().length() < 10) {
			Toast.makeText(this,
					"El número telefónico debe contener 10 dígitos",
					Toast.LENGTH_LONG).show();
			txtNumCell.requestFocus();
			return;
		}

		if (!txtNumCell.getText().toString()
				.matches("^[0-9]{2,3}-? ?[0-9]{6,7}$")) {
			Toast.makeText(this,
					"Introducir un número telefónico válido de 10 dígitos",
					Toast.LENGTH_LONG).show();
			txtNumCell.requestFocus();
			return;
		}

		if (txtUsuario.getText().toString().length() < 3) {
			Toast.makeText(this,
					"Debe introducir el nombre del usuario responsable",
					Toast.LENGTH_LONG).show();
			txtUsuario.requestFocus();
			return;
		}

		if (txtUri.getText().length() < 1) {
			Toast.makeText(this, "Debe introducir una url", Toast.LENGTH_LONG)
					.show();
			txtUri.requestFocus();
			return;
		}

		Resources res = getResources();

		if (!txtUri.getText().toString().endsWith("/")) {
			txtUri.setText(txtUri.getText().toString() + "/");
		}

		dialog = new ProgressDialog(Registro.this);
		dialog.setProgressStyle(ProgressDialog.STYLE_SPINNER);
		dialog.setMessage("Conectando al Servicio...");
		dialog.setCancelable(true);
		dialog.setMax(100);

		try {

			Moviles mov = new Moviles();
			mov.setTelefono(txtNumCell.getText().toString());
			mov.setResponsable(txtUsuario.getText().toString());
			mov.setPassword(txtPassword.getText().toString());

			this.Proveedor.URL = res.getString(R.string.prefijoServicio)
					+ txtUri.getText().toString()
					+ res.getString(R.string.posfijoServicio);
			Log.i("Registro.Proveedor.URL", this.Proveedor.URL);

			if (Registro.IsForLogin) {
				this.Proveedor.BeginValidarCelular(mov,
						new Proveedor_Callback() {
							@Override
							public void Callback(Object result) {
								if (result != null && result.toString().length() > 0) {
									ResultadoExitoso(result.toString());
								}
							}
						});
			} else {
				if(cellNumber.length() > 0){
					this.Proveedor.BeginActualizarCelular(mov,
							new Proveedor_Callback() {
								@Override
								public void Callback(Object result) {
									if (result != null && result.toString().length() > 0) {
										ResultadoExitoso(result.toString());
									}
								}
							});
				}
				else {
					this.Proveedor.BeginRegistrarCelular(mov,
							new Proveedor_Callback() {
								@Override
								public void Callback(Object result) {
									if (result != null && result.toString().length() > 0) {
										ResultadoExitoso(result.toString());
									}
								}
							});
				}
			}
		} catch (NotFoundException e) {
			Log.e("Registro.NotFoundException", e.getLocalizedMessage());
			e.printStackTrace();
		} catch (Exception e) {
			e.printStackTrace();
			Toast.makeText(this, "Error al contactar servicio",
					Toast.LENGTH_LONG).show();
			lblRes.setText("Error al contactar servicio");
		}
	}

	public void ResultadoExitoso(String resultado) {
		if (resultado.length() > 0) {
			setValorSP("Celular", txtNumCell.getText().toString());
			setValorSP("Usuario", txtUsuario.getText().toString());
			setValorSP("Password", txtPassword.getText().toString());
			setValorSP("uri", txtUri.getText().toString());
			setValorSP("Estacion", resultado);
			registrado = true;

			lblRes.setText("Estación:" + resultado);
		}
		
		Intent returnIntent = new Intent();
		
		if (registrado) {
			returnIntent.putExtra("CelRegistrado", true);
			setResult(RESULT_OK, returnIntent);
		} else {
			setResult(RESULT_CANCELED, returnIntent);
		}
		
		finish();		
	}

	// Guarda el valor de una variable en SharedPreferences
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
