
using Microsoft.AspNetCore.Mvc;
using ISO810_P2.Model;

namespace ISO810_P2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private int[] Validation = new int[17] { 1, 3, 1, 11, 50, 40, 40, 1, 16, 16, 11, 16, 16, 16, 4, 18, 18 };
        // GET: api/<FileController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", "value3", "value4" };
        }

        // GET api/<FileController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FileController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            List<string> errors = new List<string>();
            try
            {
                if (file == null)
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
                    await db.Database.EnsureCreatedAsync();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        Int64 lineCounter = 0;
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            lineCounter++;
                            var data = line.Split(",");
                            if (data.Length != Validation.Length)
                            {
                                return BadRequest("Número de columnas diferente a lo solicitado");
                            }
                            for (Int64 i = 0; i < data.Length; i++)
                            {
                                Int64 columna = i + 1;
                                if (data[i].Length > Validation[i])
                                {
                                    errors.Add("Error en línea #" + lineCounter + " columna #" + columna + " Dato: '" + data[i] + "' Debe tener una longitud de " + Validation[i] + " Caracteres");
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
                                Ingresos_Externos_ISR = data[12],
                                Salario_Infotep = data[13],
                                Tipo_Ingreso = data[14],
                                Regalia_Pascual = data[15],
                                Preaviso = data[16]
                            };
                            db.Nominas.Add(Nomina);
                        }
                        if (errors.Count > 0)
                        {
                            return BadRequest(errors);
                        }
                        else
                        {
                            await db.SaveChangesAsync();
                            return Ok("Done");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




    }
}
