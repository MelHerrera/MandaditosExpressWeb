using MandaditosExpress.Models;
using MandaditosExpress.Models.Extensions;
using MandaditosExpress.Models.Utileria;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MandaditosExpress.API
{
    public class PersonaWSController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        public JsonResult GetImagePerfilPersona(int PersonaId)
        {
            try
            {
                var Response = new ResponseWsImagenPerfil();

                var Persona = db.Personas.Find(PersonaId);

                if (Persona is null)
                    Response.Mensaje = "El identificador proporcionado es inválido";
                else
                {
                    Response.Imagen = Persona.Foto;
                    Response.Mensaje = "Se cargo correctamente la información";
                    Response.Exito = true;
                }
                return Json(Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //no revelar información confidencial
                return Json(new ResponseWsImagenPerfil { Mensaje = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Registro(Registro registro)
        {
            var response = new ResponseWsRegistro();

            if (registro is null)
            {
                response.Mensaje = "La información proporcionada es inválida";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var user = new ApplicationUser();
                var cl = new Cliente();
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                try
                {
                    user = new ApplicationUser { UserName = registro.correo, Email = registro.correo, PhoneNumber = registro.movil };

                    var UserInDb = UserManager.FindByEmail(user.Email);

                    if (UserInDb == null)//aun no esta registrado
                    {
                        var result = await UserManager.CreateAsync(user, registro.contrasena);

                        if (result.Succeeded)
                        {
                            //agregar a su correspondiente rol aqui
                            await UserManager.AddToRoleAsync(user.Id, "Cliente");//el rol cliente debio ser creado en el startup.cs

                            //Agregamos el cliente
                            cl = new Cliente
                            {
                                CorreoElectronico = registro.correo,
                                PrimerNombre = registro.nombre,
                                PrimerApellido = registro.primerApellido,
                                SegundoApellido = registro.segundoApellido,
                                Telefono = registro.movil,
                                Foto = new Utileria().getImageBytes(Request),
                                Sexo = registro.genero,
                                Direccion = registro.direccion,
                                Cedula = registro.cedula,
                                FechaIngreso = DateTime.Now,
                                EsEmpresa = registro.isEmpresa,
                                NombreDeLaEmpresa = registro.nombreEmpresa,
                                RUC = registro.numeroRUC
                            };

                            db.Clientes.Add(cl);

                            if (db.SaveChanges() > 0)
                            {
                                //si se ha guardado bien el usuario, rol y cliente entonces enviar el correo
                                //Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771
                                //Enviar correo electrónico con este vínculo
                                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                                string confirmationMessageBody = string.Format("Estimado cliente para confirmar tu cuenta, haz clic  {0}", "<a href='" + callbackUrl + "'>Aquí</a>");
                                await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta", confirmationMessageBody);

                                response.Exito = true;
                                response.Mensaje = "Se realizó correctamente el registro del cliente, revisa la bandeja de tu correo electrónico para que confirmes tu cuenta";
                            }
                        }
                    }
                    else
                    {
                        response.Exito = false;
                        response.Mensaje = "El correo electronico ingresado ya se encuentra registrado";
                    }
                }
                catch (Exception ex)
                {
                    //si sucede algun error interno entonces quitar el cliente y el usuario
                    UserManager.RemoveFromRole(user.Id, "Cliente");
                    UserManager.Delete(user);

                    if (cl.Id > 0)
                    {
                        db.Clientes.Remove(cl);
                        db.SaveChanges();
                    }

                    response.Exito = false;
                    response.Mensaje = "Ocurrio un error procesando tu solicitud!";
                }

            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(string Email)
        {
            try
            {
                var respose = new ResponseWsResetPassword();
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var user = await UserManager.FindByNameAsync(Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // No revelar que el usuario no existe o que no está confirmado
                    respose.Exito = false;
                    respose.Mensaje = "Si ha proporcionado un correo electrónico válido y confirmado, revise la bandeja de entrada de su correo para restablecer la contraseña";
                }
                else
                {
                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    string confirmationMessageBody = string.Format("Estimado cliente para restablecer la contraseña de tu cuenta, haga clic  {0}", "<a href='" + callbackUrl + "'>Aquí</a>");
                    await UserManager.SendEmailAsync(user.Id, "Restablecer contraseña", confirmationMessageBody);

                    respose.Exito = true;
                    respose.Mensaje = "Si ha proporcionado un correo electrónico válido y confirmado, revise la bandeja de entrada de su correo para restablecer la contraseña";
                }
                return Json(respose, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //no revelar información confidencial
                return Json(new ResponseWsResetPassword { Mensaje = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}