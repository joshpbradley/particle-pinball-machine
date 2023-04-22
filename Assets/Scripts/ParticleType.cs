using UnityEngine;

public class ParticleType
{
    public Color ParticleColour
    {
        get;
    }

    protected ParticleType(Color ParticleColour)
    {
        this.ParticleColour = ParticleColour;
    }
}

public class Proton : ParticleType
{
    public Proton() : base(new Color(0f / 255f, 142f / 255f, 103f / 255f)) { }
}

public class NonProton : ParticleType
{
    public NonProton() : base(new Color(0, 0, 0)) { }
}