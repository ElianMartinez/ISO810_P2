
using Microsoft.AspNetCore.Mvc;
using System;
using ISO810_P2.Model;
using System.Text;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISO810_P2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private int[] Validation = new int[12] { 2, 10, 3, 11, 40, 11, 100, 10, 10, 10, 10, 10 };
        // GET: api/<FileController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FileController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FileController>
        [HttpPost]
        public async Task<IActionResult>  Post([FromForm]IFormFile file)
        {
            List<string> errors = new List<string>();
            try
            {
                if(file == null)
                {
                     return BadRequest("Archivo no enviado");
                }
                string exst = Path.GetExtension(file.FileName).ToLower();
                if (exst != ".txt" && exst != ".csv")
                {
                    return BadRequest("Formato de archivo no compatible, por favor enviar un txt o csv");
                }
                using (var db = new SqliteDBContext())
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        Int64 lineCounter = 0;
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            lineCounter++;
                            var data = line.Split("|");
                            for (Int64 i = 0; i < data.Length; i++)
                            {
                                Int64 columna = i + 1;
                                if (data[i].Length > Validation[i])
                                {
                                    errors.Add("Error en linea #" + lineCounter + " Columna #" + columna + " Dato: '" + data[i] + "' Debe tener una longitud de " + Validation[i] + " Caracteres");
                                }

                            }
                            var Nomina = new Nomina
                            {
                                TipoRegistro = data[0],
                                ClaveNomina = data[1],
                                TipoDocumento = data[2],
                                NumeroDocumento = data[3],
                                Nombres = data[4],
                                PApellido = data[5],
                                SApellido = data[6],
                                Sexo = data[7],
                                Salario_ISR = data[8],
                                Otros_ISR = data[9],
                                RNC_Agente = data[10],
                                Remuneracio_Otros_Empleados = data[11],
                                Ingresos_Externos_ISR = "data",
                                Salario_Infotep = "info",
                                Tipo_Ingreso = "info",
                                Regalia_Pascual = "informacio",
                                Preaviso = "informacion"
                            };
                            db.Nominas.Add(Nomina);
                        }

                        await db.SaveChangesAsync();
                    }
                }
                return  Ok(errors);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }
          
        }

     

        
    }
}
