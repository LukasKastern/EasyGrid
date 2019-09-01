# EasyGrid
# CommandConsole

Easy Grid is the solution for displaying grids inside EditorWindows in Unity.

### Navigation

The Grid supports moving and zooming inside the viewport.

### Graphic Elements

Currently we have a **GridLabel** class to display text and a **GridTexture**. <br>
You can easily create your own Graphic elements by inherting by **GridGraphic** and overriding the draw method.

#### Receiving Input

Graphic Elements can receive Input by their GridEventTrigger property. <br>
Just subscribe an action to the desired EventType and you are good to go.

```csharp
EventTrigger.AddEventTrigger(TriggerEvent.OnMouseDown, onMouseDown);
```

## How-To

### Set up a grid

##### It is recommended to initialize the grid in the enable function of your EditorWindow so it can survive recompiles.

To initialize a grid all you have to do is call:

```csharp
EditorGrid.Initialize(this);
```

Than you can set the desired ViewPort of the editor that should be used. <br>
The ViewPort values are in a range of 0..1.

``` csharp
EditorGrid.SetViewPort(new ViewPort(0.1f, 0.1f, 0.8f, 0.8f));
```

You can also add an Outline to your grid by setting **EditorGrid.IsOutlineActive** to true. <br>
And specifying the Outline color by changing **EditorGrid.GridOutlineColor**.


