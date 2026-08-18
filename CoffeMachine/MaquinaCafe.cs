namespace CoffeMachine
{
    /// <summary>
    /// Representa una bebida del menú con su precio y stock disponible.
    /// Es un <c>record</c> para poder usar expresiones <c>with</c> al descontar stock.
    /// </summary>
    public record Bebida(string Nombre, int Precio, int Stock);

    /// <summary>
    /// Máquina dispensadora de café construida con TDD (ciclo Red → Green → Refactor).
    /// Cubre los casos de prueba TC-01 a TC-08 de la práctica.
    /// </summary>
    public class MaquinaCafe
    {
        private int _saldo;
        private readonly Dictionary<string, Bebida> _menu;

        /// <summary>Saldo acumulado actualmente en la máquina.</summary>
        public int Saldo => _saldo;

        public MaquinaCafe()
        {
            _menu = new()
            {
                ["Cafe"] = new("Cafe", 100, Stock: 10),
                ["Te"]   = new("Te",    75, Stock: 10),
                ["Agua"] = new("Agua",  50, Stock: 10),
            };
        }

        /// <summary>TC-01: acumula el monto insertado en el saldo.</summary>
        public void InsertarMoneda(int monto) => _saldo += monto;

        /// <summary>
        /// TC-02/03/08: intenta dispensar la bebida indicada.
        /// Devuelve <c>false</c> si no hay saldo suficiente o no queda stock.
        /// </summary>
        /// <exception cref="ArgumentException">TC-05: si la bebida no existe en el menú.</exception>
        public bool SeleccionarBebida(string nombre)
        {
            var bebida = ObtenerBebidaOLanzar(nombre);

            if (_saldo < bebida.Precio || bebida.Stock == 0)
                return false;

            _saldo -= bebida.Precio;
            _menu[nombre] = bebida with { Stock = bebida.Stock - 1 };
            return true;
        }

        private Bebida ObtenerBebidaOLanzar(string nombre)
            => _menu.TryGetValue(nombre, out var bebida)
                ? bebida
                : throw new ArgumentException($"No existe la bebida '{nombre}'");

        /// <summary>TC-04: devuelve el cambio acumulado y reinicia el saldo.</summary>
        public int ObtenerCambio()
        {
            var cambio = _saldo;
            _saldo = 0;
            return cambio;
        }

        /// <summary>TC-07: devuelve las monedas reiniciando el saldo a cero.</summary>
        public void DevolverMonedas() => _saldo = 0;

        /// <summary>TC-06: retorna el menú con las 3 bebidas y sus precios/stock.</summary>
        public Dictionary<string, Bebida> ObtenerMenu() => _menu;

        /// <summary>
        /// Nueva funcionalidad: reabastece el stock de una bebida sumando las
        /// unidades indicadas a las existentes.
        /// </summary>
        /// <exception cref="ArgumentException">Si la bebida no existe en el menú.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Si las unidades no son positivas.</exception>
        public void Reabastecer(string nombre, int unidades)
        {
            if (unidades <= 0)
                throw new ArgumentOutOfRangeException(nameof(unidades),
                    "Las unidades a reabastecer deben ser mayores que cero.");

            var bebida = ObtenerBebidaOLanzar(nombre);
            _menu[nombre] = bebida with { Stock = bebida.Stock + unidades };
        }
    }
}
