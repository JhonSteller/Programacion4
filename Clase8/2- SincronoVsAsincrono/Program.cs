using System.Diagnostics;
using System.Net.Http;
using System.Threading;

class Program
{
    /// <summary>
    /// Este código muestra una comparativa de tiempo entre procesos sincronos y asincronos en .Net!!
    /// </summary>
    /// <returns></returns>
    static async Task Main()
    {
        Console.WriteLine("=== DEMOSTRACIÓN DE PROCESOS ASÍNCRONOS EN .NET 10 ===\n");

        await Ejemplo1_SincronoVsAsincrono();

        Console.WriteLine("\n=== FIN DE LA DEMOSTRACIÓN ===");
        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
    static async Task Ejemplo1_SincronoVsAsincrono()
    {
        Console.WriteLine("EJEMPLO 1: Código Síncrono vs Asíncrono ---");

        // VERSIÓN SÍNCRONA
        Console.WriteLine("Ejecutando versión SÍNCRONA:");
        var sw = Stopwatch.StartNew();

        InvocarBaseDeDatos();      // Espera 3 segundos
        LecturaDeDisco();         // Espera 2 segundos
        InvocacionServicioWeb();       // Espera 4 segundos

        sw.Stop();
        Console.WriteLine($"Tareas finalizadas  (Síncrono): {sw.ElapsedMilliseconds}ms\n");

        // VERSIÓN ASÍNCRONA
        Console.WriteLine("Ejecutando versión ASÍNCRONA:");
        sw.Restart();

        // Iniciamos todas las tareas al mismo tiempo
        Task InvocarABDTask = InvocarBaseDeDatosAsync();
        Task LecturaDiscoTask = LecturaDeDiscoAsync();
        Task InvocarServicioWebTask = InvocacionServicioWebAsync();

        // Esperamos a que todas terminen
        await Task.WhenAll(InvocarABDTask, LecturaDiscoTask, InvocarServicioWebTask);

        sw.Stop();
        Console.WriteLine($"Tareas finalizadas (Asíncrono): {sw.ElapsedMilliseconds}ms");
        Console.WriteLine("Nota: La versión asíncrona es mucho más rápida porque las tareas se ejecutan en paralelo");
    }

    // Métodos síncronos (bloquean el hilo)
    static void InvocarBaseDeDatos()
    {
        Console.WriteLine("Invocando base de datos...");
        Thread.Sleep(3000); // Simula 3 segundos de trabajo
        Console.WriteLine("Invocación a base de datos lista");
    }

    static void LecturaDeDisco()
    {
        Console.WriteLine("Leyendo en disco...");
        Thread.Sleep(2000); // Simula 2 segundos de trabajo
        Console.WriteLine("Lectura en disco completa");
    }

    static void InvocacionServicioWeb()
    {
        Console.WriteLine("Invocando servicio web...");
        Thread.Sleep(4000); // Simula 4 segundos de trabajo
        Console.WriteLine("Invocación a servicio web finalizada");
    }

    // Métodos asíncronos (no bloquean el hilo)
    static async Task InvocarBaseDeDatosAsync()
    {
        Console.WriteLine("Invocando base de datos(async)...");
        await Task.Delay(3000); // Espera asíncrona de 3 segundos
        Console.WriteLine("Invocación a base de datos lista");
    }

    static async Task LecturaDeDiscoAsync()
    {
        Console.WriteLine("Leyendo en disco(async)...");
        await Task.Delay(2000); // Espera asíncrona de 2 segundos
        Console.WriteLine("Lectura en disco completa");
    }

    static async Task InvocacionServicioWebAsync()
    {
        Console.WriteLine("Invocando servicio web(async)...");
        await Task.Delay(4000); // Espera asíncrona de 4 segundos
        Console.WriteLine("Invocación a servicio web finalizada");
    }

}
