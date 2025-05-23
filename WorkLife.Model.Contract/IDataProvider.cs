
namespace WorkLife.Model.Contract
{
    /// <summary>
    /// Provides access to the data that is extracted from the DB
    /// Can handle required data transformations
    /// </summary>
    public interface IDataProvider
    {
        IList<IPerson> Persons { get; }

        Task<string> ExportByDate(DateOnly date);

        TargetTimeWeek GeTargetWorkingTimesByDate(TargetTimeModel targetTimeModel, DateOnly date);
    }
}
