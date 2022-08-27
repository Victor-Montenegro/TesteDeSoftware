using Xunit;

namespace Features.Tests._08___Skip
{
    public class TesteNaoPassandoMotivoEspecifico
    {
        [Fact(DisplayName = "Caso de teste sendo escapado",Skip = "Teste sendo skipado por motivos de nova versão sendo aplciada")]
        [Trait("Categoria","Teste para realizar a fuga de test")]
        public void Teste_NaoEstaPassando_VersaoNovaNaoCompativel()
        {
            Assert.True(false);
        }
    }
}