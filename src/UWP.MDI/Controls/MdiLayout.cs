namespace UWP.MDI.Controls
{
    public abstract class MdiLayout
    {
        public static MdiLayoutCascade Cascade => new MdiLayoutCascade();
        public static MdiLayoutTileVertical TileVertical => new MdiLayoutTileVertical();
        public static MdiLayoutTileHorizontal TileHorizontal => new MdiLayoutTileHorizontal();

        public abstract void RunLayout(MDIContainer container);
    }
}