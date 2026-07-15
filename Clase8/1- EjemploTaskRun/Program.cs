using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WhenAnyDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=====Task.WhenAll (espera todos) =====\n");
            await SinWhenAny();

            Console.WriteLine("\n\n=====Task.WhenAny (espera el primero) =====\n");
            await ConWhenAny();
        }

        static async Task SinWhenAny()
        {
            var stopwatch = Stopwatch.StartNew();

            var hilo1 = SimularServicioAsync("Servicio A", 3000);
            var hilo2 = SimularServicioAsync("Servicio B", 5000);
            var hilo3 = SimularServicioAsync("Servicio C", 7000);
            Console.WriteLine("\nEsperando a que todos terminen...");
            var resultados = await Task.WhenAll(hilo1, hilo2, hilo3);

            stopwatch.Stop();

            Console.WriteLine("\nTodos terminaron:");
            foreach (var r in resultados)
            {
                Console.WriteLine(r);
            }

            Console.WriteLine($"\nTiempo total: {stopwatch.ElapsedMilliseconds} ms");
        }

        static async Task ConWhenAny()
        {
            var stopwatch = Stopwatch.StartNew();

            var hilo1 = SimularServicioAsync("Servicio A", 5000);
            var hilo2 = SimularServicioAsync("Servicio B", 7000);
            var hilo3 = SimularServicioAsync("Servicio C", 9000);
            Console.WriteLine("\nEsperando a que el primero termine...");
            var primeraTarea = await Task.WhenAny(hilo1, hilo2, hilo3);

            var resultado = await primeraTarea;

            stopwatch.Stop();

            Console.WriteLine($"\nPrimero en responder: {resultado}");
            Console.WriteLine($"\nTiempo total: {stopwatch.ElapsedMilliseconds} ms");
        }

        static async Task<string> SimularServicioAsync(string nombre, int delay)
        {
            Console.WriteLine($"{nombre} iniciado (hilo {Environment.CurrentManagedThreadId})");

            await Task.Delay(delay);

            Console.WriteLine($"{nombre} terminado (hilo {Environment.CurrentManagedThreadId})");

            return $"{nombre} completado en {delay} ms";
        }
    }
}
