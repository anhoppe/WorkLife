
using WorkLife.Model.Contract;

namespace WorkLife.Dal.Contract
{
    /// <summary>
    /// Interface for database access.
    /// </summary>
    public interface IDbAccess
    {
        /// <summary>
        /// Connects to the DB
        /// </summary>
        /// <returns>true on successful connection</returns>
        bool Connect();

        /// <summary>
        /// Loads all persons from the DB
        /// </summary>
        /// <returns></returns>
        IList<IPerson> LoadPersons(IPersonBuilder personBuilder);

        /// <summary>
        /// Loads all working weeks for a given date
        /// </summary>
        /// <param name="date">The date of the working week</param>
        /// <returns>All working weeks for the given date</returns>
        IList<TargetTimeWeek> LoadTargetTimeWeeks();
    }
}
