using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerInteraction>().FromComponentInHierarchy().AsSingle();

        Container.Bind<ChestGridController>().FromComponentInHierarchy().AsSingle();

        Container.Bind<DragDropOrchestrator>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<ChestService>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<InventoryService>().AsSingle().NonLazy();

        Container.Bind<ItemContextMenu>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ItemTooltip>().FromComponentInHierarchy().AsSingle();
    }
}
