namespace CoffeMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var maquina = new MaquinaCafe();

            Console.WriteLine("=== Máquina de Café (TDD) ===\n");

            Console.WriteLine("Menú disponible:");
            foreach (var (nombre, bebida) in maquina.ObtenerMenu())
                Console.WriteLine($"  - {nombre,-5} | Precio: {bebida.Precio,3} | Stock: {bebida.Stock}");

            Console.WriteLine("\nInsertando 150...");
            maquina.InsertarMoneda(150);
            Console.WriteLine($"Saldo actual: {maquina.Saldo}");

            Console.WriteLine("\nSeleccionando 'Cafe' (precio 100)...");
            bool dispensado = maquina.SeleccionarBebida("Cafe");
            Console.WriteLine(dispensado ? "  [OK] Café dispensado." : "  [X] No se pudo dispensar.");

            Console.WriteLine($"\nCambio devuelto: {maquina.ObtenerCambio()}");
            Console.WriteLine($"Saldo tras retirar cambio: {maquina.Saldo}");

            Console.WriteLine("\n¡Disfrute su café! ☕");
        }
    }
}
