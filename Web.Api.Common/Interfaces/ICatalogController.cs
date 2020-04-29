using Web.Api.Common.Http;

namespace Web.Api.Common.Interfaces
{
    /// <summary>
    /// Interfaz Generica para Implementar en Catalogos
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TFilters"></typeparam>
    public interface ICatalogController<in TRequest,in TFilters>
    {
        /// <summary>
        /// Función para Solicitar registros de un Catálogo
        /// </summary>
        /// <param name="filters"></param>
        /// <returns>
        /// </returns>
        HttpResponseJson GetData(TFilters filters);

        /// <summary>
        /// Función para Agregar o Actualizar Datos de un Catálogo
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// </returns>
        HttpResponseJson SaveData(TRequest request);

        /// <summary>
        /// Función para Eliminar un dato de un Catálogo
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        HttpResponseJson DeleteOne(int ID);
    }
}
