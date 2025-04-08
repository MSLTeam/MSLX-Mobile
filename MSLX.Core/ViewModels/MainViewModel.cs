﻿using Material.Icons.Avalonia;
using Material.Icons;
using SukiUI.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using SukiUI.Dialogs;

namespace MSLX.Core.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public static ISukiDialogManager DialogManager { get; } = new SukiDialogManager();
    public static ServerListViewModel ServerListViewModel { get; } = new ServerListViewModel();
    
    private readonly ObservableCollection<SukiSideMenuItem> _mainPages = new()
    {
        new SukiSideMenuItem
        {
            Header = "主页",
            Icon = new MaterialIcon()
            {
                Kind = MaterialIconKind.Home,
            },
            PageContent = new HomeViewModel(),
        },
        new SukiSideMenuItem
        {
            Header = "服务器列表",
            Icon = new MaterialIcon()
            {
                Kind = MaterialIconKind.ViewList,
            },
            PageContent = ServerListViewModel,
        },
        new SukiSideMenuItem
        {
            Header = "内网映射",
            Icon = new MaterialIcon()
            {
                Kind = MaterialIconKind.NavigationVariant,
            },
            PageContent = new AboutViewModel(),
        },
        new SukiSideMenuItem
        {
            Header = "点对点联机",
            Icon = new MaterialIcon()
            {
                Kind = MaterialIconKind.SwapHorizontalBold,
            },
            PageContent = new AboutViewModel(),
        },
        new SukiSideMenuItem
        {
            Header = "设置",
            Icon = new MaterialIcon()
            {
                Kind = MaterialIconKind.Settings,
            },
            PageContent = new SettingsViewModel(),
        },
        new SukiSideMenuItem
        {
            Header = "关于",
            Icon = new MaterialIcon()
            {
                Kind = MaterialIconKind.Info,
            },
            PageContent = new AboutViewModel(),
        }
    };

    public ObservableCollection<SukiSideMenuItem> MainPages
    {
        get => _mainPages;
    }

    private SukiSideMenuItem? _activePage;
    public SukiSideMenuItem? ActivePage
    {
        get => _activePage;
        set
        {
            _activePage = value;
            OnPropertyChanged();
        }
    }

    public static void NavigateTo<T>() where T : ViewModelBase
    {
        if (App.Instance == null) return;

        var page = App.Instance.MainPages.FirstOrDefault(p => p.PageContent is T);
        if (page != null)
            App.Instance.ActivePage = page;
    }

    public static void NavigateTo(ViewModelBase viewModel)
    {
        if (App.Instance == null) return;

        // 查找是否已存在该类型的页面
        var page = App.Instance.MainPages.FirstOrDefault(p => p.PageContent.GetType() == viewModel.GetType());
        if (page == null)
        {
            // 创建新页面
            page = new SukiSideMenuItem
            {
                Header = viewModel.GetType().Name.Replace("ViewModel", ""),
                PageContent = viewModel
            };
            App.Instance.MainPages.Add(page);
        }
        App.Instance.ActivePage = page;
    }

    public MainViewModel()
    {
        ActivePage = MainPages.FirstOrDefault();
    }
}
