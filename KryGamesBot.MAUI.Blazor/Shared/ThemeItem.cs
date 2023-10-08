namespace KryGamesBot.MAUI.Blazor.Shared {
    public record ThemeItem(string Name, string[] StylesheetLinkUrl) {
        public static readonly ThemeItem Default = Create("blazing-berry");
        public static ThemeItem Create(string name) {
            if (Utils.DevExpressThemes.Contains(name))
                return new ThemeItem(name, new[] { $"_content/DevExpress.Blazor.Themes/{name}.bs5.min.css" });
            return new ThemeItem(name, new[] { "_content/DevExpress.Blazor.Themes/bootstrap-external.bs5.min.css", $"css/switcher-resources/themes/{name}/bootstrap.min.css" });
        }
    };
}