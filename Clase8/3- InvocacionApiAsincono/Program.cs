using System.Diagnostics;

namespace InvocacionApiAsincono
{
    internal class Program
    {
        /// <summary>
        /// Este metodo realiza un lamado a un api en internet y mientras espera la respuesta pinta en pantalla un cargando
        /// el async evita la ejecución del hilo principal
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();

            var cargandoTask = MostrarCargandoAsync(cts.Token);

            var llamadoAlApiTask = LlamadoAlApiPorVariasCedulasAsync();
            var llamadoAlApiPorCedulaTask = LlamadoPorCedulAsync();

            var laListaDeTareas = new List<Task>();
            laListaDeTareas.Add(llamadoAlApiTask);
            laListaDeTareas.Add(llamadoAlApiPorCedulaTask);

            await Task.WhenAll(laListaDeTareas);
         
            var resultadoLlamado1 = llamadoAlApiTask.Result;
            var resultadoAConsultaPorCedula = llamadoAlApiPorCedulaTask.Result;

            // Detenemos el loader
            cts.Cancel();
            await cargandoTask;

            Console.WriteLine("\n\nRespuesta del API:");
            Console.WriteLine(resultadoLlamado1);

            Console.WriteLine("               ");
            Console.WriteLine("Resultado llamado 2: ");
            Console.WriteLine(resultadoAConsultaPorCedula);
        }
        static async Task<string> LlamadoPorCedulAsync()
        {
            using var elCLiente = new HttpClient();

            //Debido a que este api responde lento no hace falta simular
            var laRespuestaDelApi = await elCLiente.GetAsync("http://5.161.187.235/padron/api/DistritoElectoral/113710411");
            laRespuestaDelApi.EnsureSuccessStatusCode();

            return await laRespuestaDelApi.Content.ReadAsStringAsync();
        }

        static async Task<string> LlamadoAlApiPorVariasCedulasAsync()
        {
            using var elCLiente = new HttpClient();

            //Debido a que este api responde lento no hace falta simular
            var laRespuestaDelApi = await elCLiente.GetAsync("http://5.161.187.235/padron/api/DistritoElectoral/");
            laRespuestaDelApi.EnsureSuccessStatusCode();

            return await laRespuestaDelApi.Content.ReadAsStringAsync();
        }

        static async Task MostrarCargandoAsync(CancellationToken token)
        {
            var secuencia = new[] { '|', '/', '-', '\\' };
            int index = 0;

            try
            {
                while (true)
                {
                    Console.Write($"\rCargando {secuencia[index++ % secuencia.Length]}");
                    await Task.Delay(100, token);
                }
            }
            catch (TaskCanceledException)
            {
                // Cancelación esperada. No hacer nada.
            }

            Console.Write("\r                     \r");
        }
    }
}
