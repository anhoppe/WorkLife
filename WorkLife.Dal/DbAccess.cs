
using Microsoft.Data.SqlClient;
using WorkLife.Dal.Contract;
using WorkLife.Model.Contract;

namespace WorkLife.Dal
{
    public class DbAccess : IDbAccess
    {
        private readonly string _connectionString = "Server=HOPPESSUISSEDEV\\SQLEXPRESS;Database=TEST;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

        private SqlConnection? _connection;

        ~DbAccess()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public bool Connect()
        {
            try
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            catch (ArgumentException)
            {
                // ToDo: Error handling for failing to opening the connection
                return false;
            }

            return true;
        }

        public IList<IPerson> LoadPersons(IPersonBuilder personBuilder)
        {
            var persons = new List<IPerson>();

            try
            {
                using var userSql = new SqlCommand("SELECT Id, Name, Personalnummer FROM Personen", _connection);
                using var reader = userSql.ExecuteReader();

                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var name = reader.GetString(1);
                    var employeeId = reader.GetString(2);

                    // This could potentially be improved by returning a list of Person DTO
                    // Then the model could construct the IPersion object from the DTO
                    // and we didn't need to pass the factory into the method
                    personBuilder = personBuilder.WithName(name).
                        WithId(id).
                        WithEmployeeId(employeeId);

                    // Not really nice, quick-hack to get the target time model data for the person
                    // Might make more sense to cache the table in a dictionary beforehand
                    var timeModelId = 0;
                    var validStartDate = DateOnly.MinValue;

                    using var timeModelSql = new SqlCommand($"SELECT SollzeitModellId, GueltigAb FROM PersonenSollzeitModelle WHERE PersonenId = {id}", _connection);
                    using var timeModelReader = timeModelSql.ExecuteReader();

                    while (timeModelReader.Read())
                    {
                        timeModelId = timeModelReader.GetInt32(0);
                        validStartDate = DateOnly.FromDateTime(timeModelReader.GetDateTime(1));
                        personBuilder = personBuilder.WithTargetTimeModel(validStartDate, timeModelId.ToTargetTimeModel());
                    }

                    persons.Add(personBuilder.Build());
                }

                return persons;
            }
            catch (InvalidOperationException)
            {
                // ToDo: Error handling, DB was not opened
                // It would be better to notify the user that something went wrong
                // but since error handling is not really defined we just return an empty list
                return persons;
            }
        }

        public IList<TargetTimeWeek> LoadTargetTimeWeeks()
        {
            var targeTimeWeeks = new List<TargetTimeWeek>();

            try
            {
                using var userSql = new SqlCommand("SELECT * FROM SollzeitModelleZeiten", _connection);
                using var reader = userSql.ExecuteReader();

                while (reader.Read())
                {
                    var workingWeek = new TargetTimeWeek();

                    workingWeek.TargetTimeModel = reader.GetInt32(0).ToTargetTimeModel();
                    workingWeek.ValidFromDate = DateOnly.FromDateTime(reader.GetDateTime(1));
                    workingWeek.TargetWorkingTimeMin[(int)DayOfWeek.Monday] = reader.GetInt16(2);
                    workingWeek.TargetWorkingTimeMin[(int)DayOfWeek.Tuesday] = reader.GetInt16(3);
                    workingWeek.TargetWorkingTimeMin[(int)DayOfWeek.Wednesday] = reader.GetInt16(4);
                    workingWeek.TargetWorkingTimeMin[(int)DayOfWeek.Thursday] = reader.GetInt16(5);
                    workingWeek.TargetWorkingTimeMin[(int)DayOfWeek.Friday] = reader.GetInt16(6);
                    workingWeek.TargetWorkingTimeMin[(int)DayOfWeek.Saturday] = reader.GetInt16(7);
                    workingWeek.TargetWorkingTimeMin[(int)DayOfWeek.Sunday] = reader.GetInt16(8);

                    targeTimeWeeks.Add(workingWeek);
                }
            }
            catch (InvalidOperationException)
            {
                // ToDo: Error handling
            }

            return targeTimeWeeks;
        }
    }
}
