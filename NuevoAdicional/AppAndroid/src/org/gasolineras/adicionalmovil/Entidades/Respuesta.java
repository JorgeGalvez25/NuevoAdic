package org.gasolineras.adicionalmovil.Entidades;

import java.util.Hashtable;

import org.ksoap2.serialization.KvmSerializable;
import org.ksoap2.serialization.PropertyInfo;

public class Respuesta implements KvmSerializable {

	public static Class RESPUESTA_CLASS = Respuesta.class;

	private Boolean IsFaulted;

	private String Message;

	private Object Result;

	public Respuesta() {
		this.IsFaulted = false;
		this.Message = "";
		this.Result = null;
	}

	@Override
	public Object getProperty(int arg0) {
		switch (arg0) {
		case 0:
			return IsFaulted;
		case 1:
			return Message;
		case 2:
			return Result;
		}
		return null;
	}

	@Override
	public int getPropertyCount() {
		return 3;
	}

	@Override
	public void getPropertyInfo(int arg0, Hashtable arg1, PropertyInfo arg2) {
		switch (arg0) {
		case 0:
			arg2.type = PropertyInfo.BOOLEAN_CLASS;
			arg2.name = "IsFaulted";
			break;
		case 1:
			arg2.type = PropertyInfo.STRING_CLASS;
			arg2.name = "Message";
			break;
		case 2:
			arg2.type = PropertyInfo.OBJECT_CLASS;
			arg2.name = "Result";
			break;
		}
	}

	@Override
	public void setProperty(int arg0, Object arg1) {
		switch (arg0) {
		case 0:
			IsFaulted = Boolean.parseBoolean(arg1.toString());
			break;
		case 1:
			Message = arg1.toString();
			break;
		case 2:
			Result = arg1;
			break;
		}
	}

	public Boolean getIsFaulted() {
		return IsFaulted;
	}

	public void setIsFaulted(Boolean isFaulted) {
		IsFaulted = isFaulted;
	}

	public String getMessage() {
		return Message;
	}

	public void setMessage(String message) {
		Message = message;
	}

	public Object getResult() {
		return Result;
	}

	public void setResult(Object result) {
		Result = result;
	}

}
