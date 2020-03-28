using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace BetterSpeed
{
    [StaticConstructorOnStartup]
    public static class Compatibility_GiddyUp
    {
        static Compatibility_GiddyUp()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("GiddyUpCore"));
            if(assembly != null)
            {
                Log.Message($"BetterSpeed: GiddyUp Detected!");
                var type = assembly.DefinedTypes.FirstOrDefault(x => x.Name == "IsMountableUtility");
                var method = type?.GetMethod("CurMount");
                if(method != null)
                {
                    var paramExpr = Expression.Parameter(typeof(Pawn), "rider");
                    GetMount = Expression.Lambda<Func<Pawn, Pawn>>(Expression.Call(method, paramExpr), paramExpr).Compile();

                    Log.Message($"BetterSpeed: GiddyUp Compatibility! (1/2)");
                }
                var jobDriverType = assembly.DefinedTypes.FirstOrDefault(x => x.Name == "JobDriver_Mounted");
                if(jobDriverType != null)
                {
                    var paramExpr = Expression.Parameter(typeof(Pawn), "mount");
                    var jobDriverExpr = Expression.Field(Expression.Field(paramExpr, nameof(Pawn.jobs)), "curDriver");
                    var conditionExpr = Expression.Condition(
                            Expression.AndAlso(
                                Expression.NotEqual(Expression.Field(paramExpr, nameof(Pawn.jobs)), Expression.Default(typeof(Pawn_JobTracker))),
                                Expression.TypeIs(jobDriverExpr, jobDriverType)),
                            Expression.Property(Expression.Convert(jobDriverExpr, jobDriverType), "Rider"),
                            Expression.Default(typeof(Pawn)));
                    GetRider = Expression.Lambda<Func<Pawn, Pawn>>(conditionExpr, paramExpr).Compile();
                    Log.Message($"BetterSpeed: GiddyUp Compatibility! (2/2)");
                }
            }
        }


        public static Func<Pawn, Pawn> GetMount { get; private set; } = p => null;
        public static Func<Pawn, Pawn> GetRider { get; private set; } = p => null;
    }
}
