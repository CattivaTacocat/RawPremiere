using System.Collections.Generic;

namespace DeadDog.RecallPast.Libs.Nodeo.Input;

public record NodeoInputMapDto
{
    public Dictionary<string, NodeoInputDto[]> InputMap { get; set; }
}