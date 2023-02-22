using Adicional.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Web.Adicional;
using System.Collections.Generic;
using System.Linq;

namespace ImagenSoft.ModuloWeb.Persistencia.Persistencia.Web.Adicional
{
    public class ManguerasPersistencia
    {
        //private static Regex MatchCombustible = new Regex(@"(^[diesel|premium|magna])\w+", RegexOptions.Compiled |
        //                                                                                   RegexOptions.Multiline |
        //                                                                                   RegexOptions.IgnoreCase);
        public ListaDispensarios ObtenerPosiciones(MarcaDispensario marca, ListaHistorial historial)
        {
            ListaDispensarios lstResult = null;
            IEnumerable<int> lstPosiciones = historial.AsParallel()
                                               .Select(p => p.Posicion)
                                               .Distinct()
                                               .OrderBy(p => p);

            switch (marca)
            {
                case MarcaDispensario.Wayne:
                    lstResult = llenarListaWayne(lstPosiciones, historial);
                    break;
                case MarcaDispensario.Bennett:
                    lstResult = llenarListaBennet(lstPosiciones, historial);
                    break;
                case MarcaDispensario.Team:
                    lstResult = llenarListaTeam(lstPosiciones, historial);
                    break;
                case MarcaDispensario.Gilbarco:
                    lstResult = llenarListaGilbarco(lstPosiciones, historial);
                    break;
                case MarcaDispensario.HongYang:
                    lstResult = llenarListaHongYang(lstPosiciones, historial);
                    break;
                case MarcaDispensario.Ninguno:
                default:
                    lstResult = new ListaDispensarios();
                    break;
            }

            return lstResult;
        }

        #region Obtener Lista

        private ListaDispensarios llenarListaWayne(IEnumerable<int> listaPosiciones, ListaHistorial mangueras)
        {
            string[] nombresCombustibles = new string[] { string.Empty, "Magna", "Premium", "Diesel" };
            Dictionary<short, bool> combustibles = new Dictionary<short, bool>();

            ListaDispensarios lstResult = new ListaDispensarios();
            //listaPosiciones.ForEach(p =>
            foreach (var p in listaPosiciones)
            {
                mangueras.AsParallel()
                         .Where(q => q.Posicion == p) // Hasta aqui es Paralelo
                         .ToList() // Se cambia a sincrono para validar los valores del diccionario
                         .ForEach(m =>
                         {
                             if (!combustibles.ContainsKey(m.Combustible))
                             {
                                 combustibles.Add(m.Combustible, true);
                                 lstResult.Add(new Dispensarios()
                                     {
                                         id = m.Manguera,
                                         estacion = m.Id_Estacion,
                                         dispensario = p,
                                         posicion = p,
                                         combustible = m.Combustible,
                                         nombre = nombresCombustibles[m.Combustible],
                                         valor = decimal.ToDouble(m.Porcentaje),
                                         showTitleId = false
                                     });
                             }
                         });
            }

            return lstResult;
        }

        private ListaDispensarios llenarListaBennet(IEnumerable<int> listaPosiciones, ListaHistorial mangueras)
        {
            ListaDispensarios lstResult = new ListaDispensarios();
            lstResult.AddRange((from p in listaPosiciones.AsParallel()
                                select (from i in mangueras.AsParallel()
                                        where i.Posicion == p
                                        select new Dispensarios()
                                            {
                                                id = i.Manguera,
                                                estacion = i.Id_Estacion,
                                                posicion = p,
                                                combustible = i.Combustible,
                                                nombre = i.Calibracion == 0
                                                            ? string.Format("Manguera {0:D2}", i.Manguera)
                                                            : string.Format("Manguera {0:D2} - Calibración ({1:N2})", i.Manguera, i.Calibracion),
                                                valor = decimal.ToDouble(i.Porcentaje),
                                                showTitleId = false
                                            })).SelectMany(q => q).OrderBy(q => q.posicion));
            //foreach (var p in listaPosiciones)
            //{
            //    lstResult.AddRange(from i in mangueras.AsParallel()
            //                       where i.Posicion == p
            //                       select new Dispensarios()
            //                           {
            //                               id = i.Manguera,
            //                               estacion = i.Id_Estacion,
            //                               posicion = p,
            //                               combustible = i.Combustible,
            //                               nombre = i.Calibracion == 0
            //                                           ? string.Format("Manguera {0:D2}", i.Manguera)
            //                                           : string.Format("Manguera {0:D2} - Calibración ({1:N2})", i.Manguera, i.Calibracion),
            //                               valor = decimal.ToDouble(i.Porcentaje),
            //                               showTitleId = false
            //                           });


            //    //mangueras.AsParallel()
            //    //         .Where(q => q.Posicion == p)
            //    //         .ForAll(m =>
            //    //         {
            //    //             lstResult.Add(new Dispensarios()
            //    //                 {
            //    //                     id = m.Manguera,
            //    //                     estacion = m.Id_Estacion,
            //    //                     posicion = p,
            //    //                     combustible = m.Combustible,
            //    //                     nombre = m.Calibracion == 0
            //    //                                 ? string.Format("Manguera {0:D2}", m.Manguera)
            //    //                                 : string.Format("Manguera {0:D2} - Calibración ({1:N2})", m.Manguera, m.Calibracion),
            //    //                     valor = decimal.ToDouble(m.Porcentaje),
            //    //                     showTitleId = false
            //    //                 });
            //    //         });
            //}

            return lstResult;
        }

        private ListaDispensarios llenarListaTeam(IEnumerable<int> listaPosiciones, ListaHistorial mangueras)
        {
            ListaDispensarios lstResult = new ListaDispensarios();
            int dispensario = 1;

            lstResult.AddRange(from i in listaPosiciones.AsParallel()
                               where ((i % 2) == 1)
                               let noDisp = System.Threading.Interlocked.Increment(ref dispensario)
                               let manguera = mangueras.AsParallel()
                                                       .Where(q => q.Posicion == i)
                                                       .OrderBy(o => o.Manguera)
                                                       .FirstOrDefault()
                               select new Dispensarios()
                               {
                                   id = manguera == null ? 0 : manguera.Manguera,
                                   estacion = manguera == null ? 0 : manguera.Id_Estacion,
                                   dispensario = noDisp,
                                   posicion = noDisp,
                                   combustible = manguera == null ? 0 : manguera.Combustible,
                                   nombre = "Cambiar...",
                                   valor = manguera == null ? 0D : decimal.ToDouble(manguera.Porcentaje),
                                   showTitleId = false
                               });

            //Historial manguera = null;
            //listaPosiciones.AsParallel()
            //              .Where(p => p % 2 == 1)
            //              .ForAll(p =>
            //    {
            //        //if ((p % 2) == 1)
            //        //{
            //        manguera = mangueras.AsParallel()
            //                            .Where(q => q.Posicion == p)
            //                            .OrderBy(o => o.Manguera)
            //                            .FirstOrDefault();
            //        lstResult.Add(new Dispensarios()
            //            {
            //                id = manguera == null ? 0 : manguera.Manguera,
            //                estacion = manguera == null ? 0 : manguera.Id_Estacion,
            //                dispensario = dispensario,
            //                posicion = dispensario,
            //                combustible = manguera == null ? 0 : manguera.Combustible,
            //                nombre = "Cambiar...",
            //                valor = manguera == null ? 0D : decimal.ToDouble(manguera.Porcentaje),
            //                showTitleId = false
            //            });
            //        System.Threading.Interlocked.Increment(ref dispensario);
            //        //}
            //    });
            return lstResult;
        }

        private ListaDispensarios llenarListaGilbarco(IEnumerable<int> listaPosiciones, ListaHistorial mangueras)
        {
            ListaDispensarios lstResult = new ListaDispensarios();

            string[] nombresCombustibles = new string[] { string.Empty, "Gasolina", "Gasolina", "Diesel" };
            Dictionary<short, bool> combustibles = new Dictionary<short, bool>();

            //listaPosiciones.ForEach(p =>
            foreach (var p in listaPosiciones)
            {
                mangueras.AsParallel()
                         .Where(q => q.Posicion == p)
                         .ToList()
                         .ForEach(m =>
                         {
                             if (!combustibles.ContainsKey(m.Combustible))
                             {
                                 combustibles.Add(m.Combustible, true);

                                 //if (m.Combustible == 1 || m.Combustible == 2)
                                 //{
                                 //    m.Combustible = 1;
                                 //}

                                 lstResult.Add(new Dispensarios()
                                     {
                                         id = m.Manguera,
                                         estacion = m.Id_Estacion,
                                         dispensario = p,
                                         combustible = (m.Combustible == 1 || m.Combustible == 2) ? 1 : m.Combustible,
                                         nombre = nombresCombustibles[m.Combustible],
                                         valor = decimal.ToDouble(m.Porcentaje),
                                         showTitleId = false
                                     });
                             }
                         });
            }
            return lstResult;
        }

        private ListaDispensarios llenarListaHongYang(IEnumerable<int> listaPosiciones, ListaHistorial mangueras)
        {
            ListaDispensarios lstResult = new ListaDispensarios();

            //listaPosiciones.ForEach(p =>
            foreach (var p in listaPosiciones)
            {
                lstResult.AddRange(from m in mangueras.AsParallel()
                                   where m.Posicion == p
                                   select new Dispensarios()
                                   {
                                       id = m.Manguera,
                                       estacion = m.Id_Estacion,
                                       dispensario = p,
                                       posicion = p,
                                       combustible = m.Combustible,
                                       nombre = string.Format("Lado {0:D2}", m.Manguera),
                                       valor = decimal.ToDouble(m.Porcentaje),
                                       showTitleId = false
                                   });
            }

            return lstResult;
        }

        #endregion
    }
}
