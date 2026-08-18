using NUnit.Framework;
using CoffeMachine;

namespace CoffeMachine.Tests
{
    [TestFixture]
    public class MaquinaCafeTests
    {
        private MaquinaCafe _maquina;

        [SetUp]
        public void Inicializar() => _maquina = new MaquinaCafe();

        // TC-01: Insertar monedas — la máquina acumula el saldo.
        [Test]
        public void InsertarMoneda_DebeAcumularSaldo()
        {
            _maquina.InsertarMoneda(25);

            Assert.That(_maquina.Saldo, Is.EqualTo(25));
        }

        // TC-02: Seleccionar bebida con saldo suficiente -> true.
        [Test]
        public void SeleccionarBebida_SaldoSuficiente_RetornaTrue()
        {
            _maquina.InsertarMoneda(100);

            Assert.That(_maquina.SeleccionarBebida("Cafe"), Is.True);
        }

        // TC-03: Saldo insuficiente -> false.
        [Test]
        public void SeleccionarBebida_SaldoInsuficiente_RetornaFalse()
        {
            _maquina.InsertarMoneda(50);

            Assert.That(_maquina.SeleccionarBebida("Cafe"), Is.False);
        }

        // TC-04: Devolver cambio correcto tras dispensar (150 - 100 = 50).
        [Test]
        public void ObtenerCambio_TrasDispensar_RetornaCambioCorrecto()
        {
            _maquina.InsertarMoneda(150);
            _maquina.SeleccionarBebida("Cafe");

            Assert.That(_maquina.ObtenerCambio(), Is.EqualTo(50));
        }

        // TC-05: Bebida no disponible -> ArgumentException.
        [Test]
        public void SeleccionarBebida_BebidaInexistente_LanzaArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _maquina.SeleccionarBebida("Jugo"));
        }

        // TC-06: El menú incluye Café=100, Té=75 y Agua=50.
        [Test]
        public void ObtenerMenu_RetornaTresBebidasConSusPrecios()
        {
            var menu = _maquina.ObtenerMenu();

            Assert.That(menu, Has.Count.EqualTo(3));
            Assert.Multiple(() =>
            {
                Assert.That(menu["Cafe"].Precio, Is.EqualTo(100));
                Assert.That(menu["Te"].Precio, Is.EqualTo(75));
                Assert.That(menu["Agua"].Precio, Is.EqualTo(50));
            });
        }

        // TC-07: Devolver monedas reinicia el saldo a cero.
        [Test]
        public void DevolverMonedas_ReiniciaSaldoACero()
        {
            _maquina.InsertarMoneda(100);

            _maquina.DevolverMonedas();

            Assert.That(_maquina.Saldo, Is.EqualTo(0));
        }

        // TC-08: Stock agotado -> no se puede dispensar (retorna false).
        [Test]
        public void SeleccionarBebida_StockAgotado_RetornaFalse()
        {
            // ARRANGE: agotar el stock inicial (10 unidades) del Café.
            for (int i = 0; i < 10; i++)
            {
                _maquina.InsertarMoneda(100);
                _maquina.SeleccionarBebida("Cafe");
            }

            // ACT: intentar comprar con saldo suficiente pero sin stock.
            _maquina.InsertarMoneda(100);
            var resultado = _maquina.SeleccionarBebida("Cafe");

            // ASSERT
            Assert.Multiple(() =>
            {
                Assert.That(resultado, Is.False);
                Assert.That(_maquina.ObtenerMenu()["Cafe"].Stock, Is.EqualTo(0));
            });
        }

        // TC-09 (nueva funcionalidad): reabastecer suma unidades al stock existente.
        [Test]
        public void Reabastecer_SumaUnidadesAlStockExistente()
        {
            // ARRANGE: el Café inicia con 10 unidades.
            _maquina.Reabastecer("Cafe", 5);

            // ACT
            var stock = _maquina.ObtenerMenu()["Cafe"].Stock;

            // ASSERT
            Assert.That(stock, Is.EqualTo(15));
        }

        // TC-10 (nueva funcionalidad): reabastecer una bebida inexistente lanza excepción.
        [Test]
        public void Reabastecer_BebidaInexistente_LanzaArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _maquina.Reabastecer("Jugo", 5));
        }

        // TC-11 (nueva funcionalidad): reabastecer con unidades no positivas lanza excepción.
        [Test]
        public void Reabastecer_UnidadesNoPositivas_LanzaArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _maquina.Reabastecer("Cafe", 0));
        }

        // Refuerzo del patrón AAA: saldo exacto descuenta el saldo a cero.
        [Test]
        public void SeleccionarBebida_SaldoExacto_DescuentaSaldo()
        {
            // ARRANGE
            _maquina.InsertarMoneda(100);

            // ACT
            var resultado = _maquina.SeleccionarBebida("Cafe");

            // ASSERT
            Assert.Multiple(() =>
            {
                Assert.That(resultado, Is.True);
                Assert.That(_maquina.Saldo, Is.EqualTo(0));
            });
        }
    }
}
