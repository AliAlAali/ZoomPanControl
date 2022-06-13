# ZoomPanControl
A user control written in c# for Windows Forms .NET environment, which supports zooming and panning.

## How to use?
You can copy ZoomControl.cs to your project and start using the User Control right away! The following examples shows a quick demonstration on its use.

```c#
var Points = new List<PointF>() {
        new PointF(10, 10),
        new PointF(10, 80),
        new PointF(100, 40),
        new PointF(10, 10),
    };

    // Define handler painting
    zoomControl1.OnDraw += ZoomControl1_OnDraw;
    
    // Optional
    zoomControl1.MinZoom = 0.1f;
    zoomControl1.MaxZoom = 20f;
    zoomControl1.ZoomSensitivity = 0.2f;
    
    // Repaint control
    zoomControl1.Invalidate();
}

// Specify what to draw here using Graphics object
private void ZoomControl1_OnDraw(object sender, ZoomControl.DrawEventArgs e)
{
    var graphics = e.Graphics;
    var pen = new Pen(Color.Black);
    graphics.DrawLines(pen, Points.ToArray());
}
```
### Control Keys
- To zoom you must press ctrl + spin mouse wheel.
- Pan hold shift + press mouse left button and drag the mouse.
- To reset to the origin state press mouse button twice.


![image](https://user-images.githubusercontent.com/8289526/173324898-e676cbbe-6d36-4e01-845e-b4f8aacad7e3.png)![image](https://user-images.githubusercontent.com/8289526/173324981-855fbd98-29cb-40f9-b479-be96ccdd6dda.png)

# License
Feel free to anything with this code.

