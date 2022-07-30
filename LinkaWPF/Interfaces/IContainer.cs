using System.Windows;

namespace LinkaWPF.Interfaces
{
    public interface IContainer
    {
        void AddElement(UIElement element);
        void RemoveElement(UIElement element);
        double Width { get; }
        double Height { get; }
    }
}
