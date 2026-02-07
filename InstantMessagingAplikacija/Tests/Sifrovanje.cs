using ClassLibrary;
namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Sifrovanje_Pomeraj1_VracaBcd()
        {
            // Arrange
            string ulaz = "abc";
            int pomeraj = 1;
            string ocekivano = "bcd";
            Sifrovanje s = new Sifrovanje();
            // Act
            string rezultat = s.Sifruj(ulaz, pomeraj);

            // Assert
            Assert.AreEqual(ocekivano, rezultat);
        }
        [Test]
        public void Sifrovanje_Pomeraj1_VracaABC()
        {
            // Arrange
            string ulaz = "bcd";
            int pomeraj = 1;
            string ocekivano = "abc";
            Sifrovanje s = new Sifrovanje();
            // Act
            string rezultat = s.Desifruj(ulaz, pomeraj);

            // Assert
            Assert.AreEqual(ocekivano, rezultat);
        }
    }
}