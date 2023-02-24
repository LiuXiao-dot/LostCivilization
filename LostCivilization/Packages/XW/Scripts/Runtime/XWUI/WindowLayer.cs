namespace XWUI
{
    /// <summary>
    /// 窗口层级设定
    /// </summary>
    public enum WindowLayer
    {
        MAIN = 0, // 一级：主窗口
        CHILD = 1, // 二级：子窗口
        PANEL = 2, // 三级：面板 ->面板的子集为面板和弹窗
        POPUP = 3, // 弹窗
        LOADING = 4, // 加载界面层
    }
}