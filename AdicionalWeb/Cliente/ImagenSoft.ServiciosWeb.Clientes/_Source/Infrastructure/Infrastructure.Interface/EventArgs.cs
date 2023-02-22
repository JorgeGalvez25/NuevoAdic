//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by this guidance package as part of the solution template
//
// For more information see: 
// ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.scsf.2007may/SCSF/html/03-01-010-How_to_Create_Smart_Client_Solutions.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using System;

namespace EstandarCliente.Infrastructure.Interface
{
    public class EventArgs<T> : EventArgs
    {
        private T _data;
        private string _modo;

        public string Modo
        {
            get { return _modo; }
            set { _modo = value; }
        }
	

        public EventArgs(T data)
        {
            _data = data;
        }

        public EventArgs(T data, string modo)
        {
            _data = data;
            _modo = modo;
        }

        public T Data
        {
            get { return _data; }
        }
    }
}