using IngresosCountry.Data;
using IngresosCountry.Models;
using Microsoft.Data.SqlClient;

namespace IngresosCountry.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly DatabaseConnection _db;

        public CatalogService(DatabaseConnection db)
        {
            _db = db;
        }

        public async Task<List<Pais>> GetPaisesAsync()
        {
            var list = new List<Pais>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT Id, Nombre, Codigo FROM tbl_paises WHERE Activo = 1 ORDER BY Nombre", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Pais
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Codigo = reader.IsDBNull(2) ? null : reader.GetString(2)
                });
            }
            return list;
        }

        public async Task<List<Area>> GetAreasAsync()
        {
            var list = new List<Area>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT Id, Nombre, Descripcion, Ubicacion FROM tbl_visitas_areas WHERE Activo = 1 ORDER BY Nombre", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Area
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Ubicacion = reader.IsDBNull(3) ? null : reader.GetString(3)
                });
            }
            return list;
        }

        public async Task<List<TipoVisita>> GetTiposVisitaAsync()
        {
            var list = new List<TipoVisita>();
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT Id, Nombre, Descripcion FROM tbl_visitas_nosocio_tipoVisita WHERE Activo = 1 ORDER BY Nombre", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new TipoVisita
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2)
                });
            }
            return list;
        }
    }
}