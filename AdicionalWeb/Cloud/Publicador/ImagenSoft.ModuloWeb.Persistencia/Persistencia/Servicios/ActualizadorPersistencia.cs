using ImagenSoft.ModuloWeb.Entidades;
using ImagenSoft.ModuloWeb.Entidades.Servicios.Actualizador;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia.Servicios
{
    public class ActualizadorPersistencia
    {
        private class FileUpdaterComparer : IEqualityComparer<FileUpdater>
        {
            bool IEqualityComparer<FileUpdater>.Equals(FileUpdater x, FileUpdater y)
            {
                return (x.FileName.Equals(y.FileName, StringComparison.OrdinalIgnoreCase) &&
                        x.MD5.Equals(y.MD5, StringComparison.OrdinalIgnoreCase));
            }

            int IEqualityComparer<FileUpdater>.GetHashCode(FileUpdater obj)
            {
                if (Object.ReferenceEquals(obj, null))
                    return 0;

                return obj.FileName.GetHashCode();
            }
        }

        private ListFileUpdater ObtenerArchivos(DirectoryInfo root, string path)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            ListFileUpdater items = new ListFileUpdater();

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles();
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
            }
            catch (System.IO.DirectoryNotFoundException ed)
            {
                //Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                if (path.StartsWith(@"\"))
                {
                    path = path.Replace(@"\", string.Empty);
                }
                Func<string, byte[]> fn = new Func<string, byte[]>((filename) =>
                {
                    using (Task<byte[]> file = this.LoadFile(filename))
                    {
                        file.Wait();
                        return file.Result;
                    }
                });

                items.AddRange(from i in files.AsParallel()
                               let data = fn(i.FullName)
                               let md5 = Utilerias.GetMD5(data)
                               select new FileUpdater()
                               {
                                   Data = data,
                                   MD5 = md5,
                                   FileName = i.Name,
                                   Path = path
                               });
                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    items.AddRange(this.ObtenerArchivos(dirInfo, dirInfo.FullName.Replace(this.TargetPath, string.Empty)));
                }
            }

            return items;
        }

        private string TargetPath;

        public ResponseUpdater ObtenerActualizaciones(RequestUpdater request)
        {
            ResponseUpdater response = new ResponseUpdater();

            try
            {
                string path = ConfigurationManager.AppSettings["Actualizador"] ?? @"C:\ImagenCo\Actualizaciones\SW";
                this.TargetPath = Path.Combine(path, request.Target);
                DirectoryInfo dir = new DirectoryInfo(path);
                if (dir.Exists)
                {
                    List<FileUpdater> withFolder = new List<FileUpdater>();
                    withFolder.AddRange(this.ObtenerArchivos(dir, dir.Root.FullName.Replace(this.TargetPath, string.Empty)));
                    /**/
                    if (request.Files.Count <= 0)
                    {
                        response.Files.AddRange(withFolder);
                    }
                    else
                    {
                        // Actualizacione
                        response.Files.AddRange(from i in request.Files.AsParallel()
                                                from j in withFolder
                                                where i.FileName.Equals(j.FileName, StringComparison.OrdinalIgnoreCase) &&
                                                      !i.MD5.Equals(j.MD5, StringComparison.OrdinalIgnoreCase)
                                                select j);
                        // Nuevos archivos
                        response.Files.AddRange(withFolder.Except(request.Files.AsParallel().Select(p => new FileUpdater()
                                {
                                    Data = p.Data,
                                    FileName = p.FileName,
                                    MD5 = p.MD5,
                                    Delete = false,
                                    Path = p.Path
                                }), new FileUpdaterComparer()));

                        List<FileUpdater> vlue = request.Files.Except(withFolder, new FileUpdaterComparer()).ToList();

                        FileUpdater f = null;
                        vlue.ForEach(p =>
                        {
                            f = response.Files.Find(q => q.FileName.Equals(p.FileName, StringComparison.OrdinalIgnoreCase));
                            if (f != null)
                            {
                                f.Delete = true;
                            }
                            else
                            {
                                f = new FileUpdater()
                                    {
                                        Delete = true,
                                        FileName = p.FileName,
                                        MD5 = p.MD5,
                                        Path = p.Path,
                                        Data = p.Data
                                    };
                                response.Files.Add(f);
                            }
                            response.Files.ForEach(q => { if (q.FileName.Equals(p.FileName, StringComparison.OrdinalIgnoreCase)) { q.Delete = true; } });
                        });
                    }
                    /**/
                    if (response.Files.Count > 0)
                    {
                        response.HaveUpdate = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return response;
        }

        private async Task<byte[]> LoadFile(string path)
        {
            FileInfo info = new FileInfo(path);
            byte[] buffer = new byte[info.Length];

            using (BufferedStream fBuffer = new BufferedStream(info.OpenRead()))
            {
                int count = 0;
                int bytesRead = 0;

                while ((bytesRead = await fBuffer.ReadAsync(buffer, count, buffer.Length - count)) > 0)
                {
                    count += bytesRead;
                }

                await fBuffer.FlushAsync();
            }

            return buffer;
        }
    }
}
