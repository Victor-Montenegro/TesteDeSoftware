namespace NerdStore.Vendas.Core.Extensions
{
    public static class ObjectExtension
    {
        public static bool IsNull(this object obj)
            => obj is null && obj == null ? true : false;
    }
}