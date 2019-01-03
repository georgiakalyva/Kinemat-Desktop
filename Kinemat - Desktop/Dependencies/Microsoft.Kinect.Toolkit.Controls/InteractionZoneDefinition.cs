using System;
using System.Windows;

/// <summary>
/// Helper class to provide common conversion operations between 
/// Physical Interaction Zone and User Interface.
/// </summary>
internal static class InteractionZoneDefinition
{
    /// <summary>
    /// Convert coordinates expressed in interaction zone coordinate space into
    /// coordinates relative to the area available for interaction in UI.
    /// </summary>
    /// <param name="izX">
    /// X-coordinate, where 0.0 corresponds to left side of interaction zone and
    /// 1.0 corresponds to right side of interaction zone.
    /// </param>
    /// <param name="izY">
    /// Y-coordinate, where 0.0 corresponds to left side of interaction zone and
    /// 1.0 corresponds to right side of interaction zone.
    /// </param>
    /// <param name="userInterfaceWidth">
    /// Width of UI area available for interaction.
    /// </param>
    /// <param name="userInterfaceHeight">
    /// Height of UI area available for interaction.
    /// </param>
    /// <param name="uiX">
    /// [out] X-coordinate, where 0.0 corresponds to left side of UI interaction
    /// area and <paramref name="userInterfaceWidth"/> corresponds to right side
    /// of UI interaction area.
    /// </param>
    /// <param name="uiY">
    /// [out] Y-coordinate, where 0.0 corresponds to top side of UI interaction
    /// area and <paramref name="userInterfaceHeight"/> corresponds to bottom side
    /// of UI interaction area.
    /// </param>
    public static void InteractionZoneToUserInterface(double izX, double izY, double userInterfaceWidth, double userInterfaceHeight, out double uiX, out double uiY)
    {
        uiX = userInterfaceWidth * izX;
        uiY = userInterfaceHeight * izY;
    }

    /// <summary>
    /// Convert coordinates relative to the area available for interaction in UI
    /// into coordinates expressed in interaction zone coordinate space.
    /// </summary>
    /// <param name="x">
    /// X-coordinate, where 0.0 corresponds to left side of UI interaction area
    /// and <paramref name="userInterfaceWidth"/> corresponds to right side of UI
    /// interaction area.
    /// </param>
    /// <param name="y">
    /// Y-coordinate, where 0.0 corresponds to top side of UI interaction area
    /// and <paramref name="userInterfaceHeight"/> corresponds to bottom side of UI
    /// interaction area.
    /// </param>
    /// <param name="userInterfaceWidth">
    /// Width of UI area available for interaction.
    /// </param>
    /// <param name="userInterfaceHeight">
    /// Height of UI area available for interaction.
    /// </param>
    /// <returns>
    /// A point in interaction zone coordinate space, where (0.0,0.0) corresponds
    /// to top-left corner and (1.0,1.0) corresponds to bottom-right corner.
    /// </returns>
    public static Point UserInterfaceToInteractionZone(double x, double y, double userInterfaceWidth, double userInterfaceHeight)
    {
        if (userInterfaceWidth <= 0 || userInterfaceHeight <= 0.0)
        {
            throw new ArgumentException("UI Width and Height must be greater than zero.");
        }

        return new Point { X = x / userInterfaceWidth, Y = y / userInterfaceHeight };
    }

    /// <summary>
    /// Convert coordinates expressed in interaction zone coordinate space into
    /// coordinates relative to the specified element.
    /// </summary>
    /// <param name="izX">
    /// X-coordinate, where 0.0 corresponds to left side of interaction zone and
    /// 1.0 corresponds to right side of interaction zone.
    /// 
    /// </param>
    /// <param name="izY">
    /// Y-coordinate, where 0.0 corresponds to top side of interaction zone and
    /// 1.0 corresponds to bottom side of interaction zone.
    /// Y-coordinate within <paramref name="element"/>, where 0.0 corresponds to
    /// top side and height of element corresponds to the bottom.
    /// </param>
    /// <param name="element">
    /// UI element that defines the input coordinate space.
    /// </param>
    /// <returns>
    /// A point relative to <paramref name="element"/>, where:
    /// <list type="bullet">
    /// <li>
    /// For X-coordinate, 0.0 corresponds to side used as horizontal origin
    /// and width of element corresponds to other side.
    /// Horizontal origin is on the left if FlowDirection is LeftToRight, and
    /// on the right if FlowDirection is RightToLeft.
    /// </li>
    /// <li>
    /// For X-coordinate, 0.0 corresponds to the top side and height of element
    /// corresponds to the bottom.
    /// </li>
    /// </list>
    /// </returns>
    public static Point InteractionZoneToElement(double izX, double izY, FrameworkElement element)
    {
        return new Point(
            element.ActualWidth * (element.FlowDirection == FlowDirection.LeftToRight ? izX : 1.0 - izX), element.ActualHeight * izY);
    }

    /// <summary>
    /// Convert coordinates relative to the specified element into coordinates
    /// expressed in interaction zone coordinate space.
    /// </summary>
    /// <param name="x">
    /// X-coordinate within <paramref name="element"/>, where 0.0 corresponds to
    /// side used as horizontal origin and width of element corresponds to other
    /// side.
    /// Horizontal origin is on the left if FlowDirection is LeftToRight, and
    /// on the right if FlowDirection is RightToLeft.
    /// </param>
    /// <param name="y">
    /// Y-coordinate within <paramref name="element"/>, where 0.0 corresponds to
    /// top side and height of element corresponds to the bottom.
    /// </param>
    /// <param name="element">
    /// UI element that defines the input coordinate space.
    /// </param>
    /// <returns>
    /// A point in interaction zone coordinate space, where (0.0,0.0) corresponds
    /// to top-left corner and (1.0,1.0) corresponds to bottom-right corner.
    /// </returns>
    public static Point ElementToInteractionZone(double x, double y, FrameworkElement element)
    {
        if (element.ActualWidth <= 0 || element.ActualHeight <= 0.0)
        {
            throw new ArgumentException("Element Width and Height must be greater than zero.");
        }

        double izX = x / element.ActualWidth;
        double izY = y / element.ActualHeight;
        return new Point { X = element.FlowDirection == FlowDirection.LeftToRight ? izX : 1.0 - izX, Y = izY };
    }

    /// <summary>
    /// Determines if user interface coordinate components are close enough to each other
    /// to be considered indistinguishable.
    /// </summary>
    /// <param name="a">
    /// First user interface coordinate component (i.e.: X or Y component).
    /// </param>
    /// <param name="b">
    /// Second user interface coordinate component (i.e.: X or Y component).
    /// </param>
    /// <returns>
    /// true if coordinate values are close enough to be considered indistinguishable.
    /// false otherwise
    /// </returns>
    public static bool AreUserInterfaceValuesClose(double a, double b)
    {
        return Math.Abs(a - b) < 0.5;
    }
}