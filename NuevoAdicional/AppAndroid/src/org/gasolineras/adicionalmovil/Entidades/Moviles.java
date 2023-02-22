package org.gasolineras.adicionalmovil.Entidades;

import java.util.Hashtable;

import org.ksoap2.serialization.KvmSerializable;
import org.ksoap2.serialization.PropertyInfo;
import org.ksoap2.serialization.SoapObject;

public class Moviles implements KvmSerializable {

	public static Class MOVILES_CLASS = Moviles.class;

	private String Telefono;

	private String Responsable;

	private String Password;

	private String Activo;

	private String PermitirCambioFlujo;

	private Permisos Permisos;

	public Moviles() {
		Telefono = "";
		Responsable = "";
		Password = "";
		Activo = "";
		PermitirCambioFlujo = "";
		Permisos = new Permisos(false);
	}

	public String getActivo() {
		return Activo;
	}

	public String getPassword() {
		return Password;
	}

	public Permisos getPermisos() {
		return Permisos;
	}

	public String getPermitirCambioFlujo() {
		return PermitirCambioFlujo;
	}

	@Override
	public Object getProperty(int arg0) {
		switch (arg0) {
		case 0:
			return Telefono;
		case 1:
			return Responsable;
		case 2:
			return Password;
		case 3:
			return Activo;
		case 4:
			return PermitirCambioFlujo;
		case 5:
			return Permisos;
		}
		return null;
	}

	@Override
	public int getPropertyCount() {
		return 6;
	}

	@Override
	public void getPropertyInfo(int arg0, Hashtable arg1, PropertyInfo arg2) {
		switch (arg0) {
		case 0:
			arg2.type = PropertyInfo.STRING_CLASS;
			arg2.name = "Telefono";
			break;
		case 1:
			arg2.type = PropertyInfo.STRING_CLASS;
			arg2.name = "Responsable";
			break;
		case 2:
			arg2.type = PropertyInfo.STRING_CLASS;
			arg2.name = "Password";
			break;
		case 3:
			arg2.type = PropertyInfo.STRING_CLASS;
			arg2.name = "Activo";
			break;
		case 4:
			arg2.type = PropertyInfo.STRING_CLASS;
			arg2.name = "PermitirCambioFlujo";
			break;
		case 5:
			arg2.type = org.gasolineras.adicionalmovil.Entidades.Permisos.PERMISOS_CLASS;
			arg2.name = "Permisos";
			break;
		default:
			break;
		}

	}

	public String getResponsable() {
		return Responsable;
	}

	public String getTelefono() {
		return Telefono;
	}

	public void setActivo(String activo) {
		Activo = activo;
	}

	public void setPassword(String password) {
		Password = password;
	}

	public void setPermisos(Permisos permisos) {
		Permisos = permisos;
	}

	public void setPermitirCambioFlujo(String permitirCambioFlujo) {
		PermitirCambioFlujo = permitirCambioFlujo;
	}

	@Override
	public void setProperty(int arg0, Object arg1) {
		switch (arg0) {
		case 0:
			Telefono = arg1.toString();
			break;
		case 1:
			Responsable = arg1.toString();
			break;
		case 2:
			Password = arg1.toString();
			break;
		case 3:
			Activo = arg1.toString();
			break;
		case 4:
			PermitirCambioFlujo = arg1.toString();
			break;
		case 5:
			// La propiedad Permisos lo toma como SoapObject
			SoapObject obj = (SoapObject) arg1;
			if (Permisos == null) {
				Permisos = new Permisos(false);
			}
			for (int i = 0; i < obj.getPropertyCount(); i++) {
				Permisos.setProperty(i, obj.getProperty(i));
			}
			break;
		default:
			break;
		}
	}

	public void setResponsable(String responsable) {
		Responsable = responsable;
	}

	public void setTelefono(String telefono) {
		Telefono = telefono;
	}
}