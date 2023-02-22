package org.gasolineras.adicionalmovil.Registros;

import java.util.Hashtable;

import org.ksoap2.serialization.KvmSerializable;
import org.ksoap2.serialization.PropertyInfo;

public class Permisos implements KvmSerializable {

	public static Class PERMISOS_CLASS = Permisos.class;

	private Boolean SubirBajar;

	public Permisos() {
		this(false);
	}

	public Permisos(Boolean subirBajar) {
		SubirBajar = subirBajar;
	}

	@Override
	public Object getProperty(int arg0) {
		switch (arg0) {
		case 0:
			return SubirBajar;
		}
		return null;
	}

	@Override
	public int getPropertyCount() {
		return 1;
	}

	@Override
	public void getPropertyInfo(int arg0, Hashtable arg1, PropertyInfo arg2) {
		switch (arg0) {
		case 0:
			arg2.type = PropertyInfo.BOOLEAN_CLASS;
			arg2.name = "SubirBajar";
			break;
		default:
			break;
		}

	}

	public Boolean getSubirBajar() {
		return SubirBajar;
	}

	@Override
	public void setProperty(int arg0, Object arg1) {
		switch (arg0) {
		case 0:
			SubirBajar = Boolean.parseBoolean(arg1.toString());
			break;
		default:
			break;
		}
	}

	public void setSubirBajar(Boolean subirBajar) {
		SubirBajar = subirBajar;
	}
}
