using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Numerics;

namespace GrenadeEffect;

public class GrenadeEffect : BasePlugin
{
    public override string ModuleName => "GrenadeEffect";
    public override string ModuleVersion => "0.0.1";
    public override string ModuleAuthor => "belom0r";

    public override void Load(bool hotReload)
    {
        //RegisterEventHandler<EventGrenadeThrown>(EventGrenadeThrownPre, HookMode.Pre);
        RegisterEventHandler<EventGrenadeThrown>(EventGrenadeThrownPost, HookMode.Post);
        //RegisterEventHandler<EventHegrenadeDetonate>(EventGrenadeThrownPre, HookMode.Pre);
        //RegisterEventHandler<EventEntityVisible>(EventEntityVisiblePre, HookMode.Pre);
        //RegisterEventHandler<EventDecoyDetonate>(EventDecoyDetonatePost, HookMode.Post);
        //RegisterEventHandler<EventGrenadeBounce>(EventGrenadeBouncePre, HookMode.Pre);

        //RegisterEventHandler<EventGameMessage>(EventGameMessagePre, HookMode.Pre);

        //RegisterListener<Listeners.OnEntitySpawned>(entity =>
        //{
        //    switch (entity.DesignerName)
        //    {
        //        case "hegrenade_projectile":
        //            {
        //                Logger.LogInformation($"OnEntitySpawned entityName = {entity.DesignerName}");

        //                //var cbaseentity = Utilities.GetEntityFromIndex<CHEGrenadeProjectile>((int)entity.Index);

        //                //if (cbaseentity == null || !cbaseentity.IsValid)
        //                //    return;

        //                //cbaseentity.Render = Color.FromArgb(255, 0, 255, 0);
        //                //cbaseentity.RenderFX = RenderFx_t.kRenderFxGlowShell;
        //                //cbaseentity.RenderMode = RenderMode_t.kRenderNormal;

        //                //cbaseentity.Glow.GlowColorOverride = Color.FromArgb(255, 0, 255, 0);
        //                //cbaseentity.Glow.GlowRange = 5000;
        //                //cbaseentity.Glow.GlowType = 3;

        //                break;
        //            }
        //        default: break;
        //    }
        //});

        //RegisterListener<Listeners.OnEntityCreated>(entity =>
        //{
        //    switch (entity.DesignerName)
        //    {
        //        case "hegrenade_projectile":
        //            {
        //                Logger.LogInformation($"OnEntityCreated entityName = {entity.DesignerName}");

        //                //var cbaseentity = Utilities.GetEntityFromIndex<CHEGrenadeProjectile>((int)entity.Index);

        //                //if (cbaseentity == null || !cbaseentity.IsValid)
        //                //    return;

        //                //cbaseentity.NextThinkTick = (int)(Server.CurrentTime + 99999.0f);
        //                //cbaseentity.Render = Color.FromArgb(255, 0, 255, 0);
        //                //cbaseentity.RenderFX = RenderFx_t.kRenderFxGlowShell;
        //                //cbaseentity.RenderMode = RenderMode_t.kRenderNormal;

        //                //cbaseentity.Glow.GlowColorOverride = Color.FromArgb(255, 0, 255, 0);
        //                //cbaseentity.Glow.GlowRange = 5000;
        //                //cbaseentity.Glow.GlowType = 3;
        //                //cbaseentity.Glow.GlowTime = Server.CurrentTime + 99999.0f;

        //                break;
        //            }
        //        default: break;
        //    }
        //});

        AddCommand("s1", "command c1", Cmd_1);

        AddCommand("s2", "command c2", Cmd_2);
        AddCommand("s3", "command c2", Cmd_3);
    }

    public void Cmd_1(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid || player.PlayerPawn == null || !player.PlayerPawn.IsValid)
            return;

        player.PrintToChat("1");

        var entities = Utilities.FindAllEntitiesByDesignerName<CBaseCSGrenadeProjectile>("hegrenade_projectile");

        foreach (var entity in entities)
        {
            if (entity == null || !entity.IsValid)
                continue;

            player.PrintToChat("2");

            if (entity.DesignerName == "hegrenade_projectile")
            {
                player.PrintToChat("3");

                var cbaseentity = Utilities.GetEntityFromIndex<CHEGrenadeProjectile>((int)entity.Index);

                if (cbaseentity == null || !cbaseentity.IsValid || cbaseentity.Thrower.Index != player.PlayerPawn.Index)
                    continue;

                player.PrintToChat("4");

                Server.NextFrame(() =>
                {
                    cbaseentity.Render = Color.FromArgb(255, 255, 0, 0);
                    cbaseentity.RenderMode = RenderMode_t.kRenderGlow;
                    cbaseentity.RenderFX = RenderFx_t.kRenderFxGlowShell;
                });

                player.PrintToChat("5");
            }
        }
    }

    public void Cmd_2(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null || !player.IsValid || player.PlayerPawn == null || !player.PlayerPawn.IsValid)
            return;

        player.PrintToChat("1");

        var entities = Utilities.GetAllEntities();

        foreach (var entity in entities)
        {
            if (entity == null || !entity.IsValid)
                continue;

            Logger.LogInformation($"DesignerName = {entity.DesignerName}");
        }
    }

    public void Cmd_3(CCSPlayerController? player, CommandInfo command)
    {
        Logger.LogInformation($"_____________________________________________________________________________");
    }

    private HookResult EventGameMessagePre(EventGameMessage @event, GameEventInfo info)
    {
        Logger.LogInformation($"EventGameMessage Target = {@event.Target} and Text = {@event.Text}");
        return HookResult.Continue;
    }

    private HookResult EventEntityVisiblePre(EventEntityVisible @event, GameEventInfo info)
    {
        Logger.LogInformation($"EventEntityVisiblePre entityName = {@event.EventName}");
        return HookResult.Continue;
    }

    private HookResult EventGrenadeThrownPre(EventHegrenadeDetonate @event, GameEventInfo info)
    {
        Logger.LogInformation($"EventHegrenadeDetonate entityName = {@event.EventName}");
        return HookResult.Continue;
    }

    private HookResult EventGrenadeThrownPre(EventGrenadeThrown @event, GameEventInfo info)
    {
        var player = @event.Userid;

        if (player == null || !player.IsValid)
            return HookResult.Continue;

        var entities = Utilities.GetAllEntities();

        foreach (var entity in entities)
        {
            if (entity == null)
                continue;

            if (entity.DesignerName != "decoy_projectile")
                continue;

            Logger.LogInformation($"EventGrenadeThrownPre entityName = {entity.DesignerName}");
        }

        return HookResult.Continue;
    }

    private HookResult EventGrenadeThrownPost(EventGrenadeThrown @event, GameEventInfo info)
    {
        var player = @event.Userid;

        if (player == null || !player.IsValid || player.PlayerPawn == null || !player.PlayerPawn.IsValid)
            return HookResult.Continue;

        var entities = Utilities.FindAllEntitiesByDesignerName<CBaseCSGrenadeProjectile>("hegrenade_projectile");

        //hegrenade_projectile
        //flashbang_projectile
        //smokegrenade_projectile
        //molotov_projectile

        foreach (var entity in entities)
        {
            if (entity == null || !entity.IsValid)
                continue;

            if (entity.DesignerName == "hegrenade_projectile")
            {
                var cbaseentity = Utilities.GetEntityFromIndex<CHEGrenadeProjectile>((int)entity.Index);

                if (cbaseentity == null || !cbaseentity.IsValid || cbaseentity.Thrower.Index != player.PlayerPawn.Index)
                    return HookResult.Continue;

                cbaseentity.NextThinkTick = (int)(Server.CurrentTime + 99999.0f);
                cbaseentity.Render = Color.FromArgb(255, 0, 255, 0);
                cbaseentity.RenderMode = RenderMode_t.kRenderGlow;
                cbaseentity.RenderFX = RenderFx_t.kRenderFxGlowShell;

                //cbaseentity.Glow.GlowColorOverride = Color.FromArgb(255, 0, 255, 0);
                //cbaseentity.Glow.GlowRange = 500;
                //cbaseentity.Glow.GlowRangeMin = 3;
                //cbaseentity.Glow.GlowType = 3;
            }
        }

        return HookResult.Continue;
    }

    private HookResult EventDecoyDetonatePost(EventDecoyDetonate @event, GameEventInfo info)
    {
        var entity = Utilities.GetEntityFromIndex<CDecoyProjectile>(@event.Entityid);

        if (entity == null || !entity.IsValid)
            return HookResult.Continue;

        //Logger.LogInformation($"EventDecoyDetonatePost entityName = {entity.DesignerName} and id = {@event.Entityid}");

        //if (glowEffects.TryGetValue(entity.Index, out CEntityInstance? glow))
        //{
        //    glow.Remove();
        //    glowEffects.Remove(entity.Index);
        //}

        return HookResult.Continue;
    }

    //[EntityOutputHook("*", "*")]
    //public HookResult OnTouchStart(CEntityIOOutput output, string name, CEntityInstance activator, CEntityInstance caller, CVariant value, float delay)
    //{
    //    Logger.LogInformation("[EntityOutputHook Attribute] ({name}, {activator}, {caller}, {delay})", name, activator.DesignerName, caller.DesignerName, delay);

    //    return HookResult.Continue;
    //}
}

