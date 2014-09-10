using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Extensions
{
    public static class GridExtensions
    {
        public static void AddAutoRowDefinition(this Grid grid)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        public static void AddStarRowDefinition(this Grid grid, int starCount = 1)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(starCount, GridUnitType.Star) });
        }

        public static void AddAutoColumnDefinition(this Grid grid)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        }

        public static void AddStarColumnDefinition(this Grid grid, int starCount = 1)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(starCount, GridUnitType.Star) });
        }
    }
}
