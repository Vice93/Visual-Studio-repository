﻿using System.Data.Entity;

namespace Oblig4.Context
{
    public class DBInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            // Test data if model changes
            // Do nothing if Db Changes for now
        }
    }
}
