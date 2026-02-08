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
            // Act
            string rezultat = Sifrovanje.Sifruj(ulaz, pomeraj);

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
          
            // Act
            string rezultat = Sifrovanje.Desifruj(ulaz, pomeraj);

            // Assert
            Assert.AreEqual(ocekivano, rezultat);
        }
    }
}