namespace BlazorCore.Data.Models;

public class AvatarModel
{
    public string? Image { get; set; }
    public string? Placeholder { get; init; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AvatarModel) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Image, Placeholder);
    }
    
    protected bool Equals(AvatarModel other)
    {
        return Image == other.Image && Placeholder == other.Placeholder;
    }
}