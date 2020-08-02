// ------------------------------------------------------------------------------
// <copyright file="BinaryConstants.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Common.Extensions;

namespace War3Net.Modeling
{
    // https://www.hiveworkshop.com/threads/mdx-specifications.240487/
    internal static class BinaryConstants
    {
#pragma warning disable SA1600 // Elements should be documented

        internal const int FixedStringSizeShort = 80;
        internal const int FixedStringSizeLong = 260;

        internal const uint VersionChunkSize = 4;
        internal const uint ModelInfoChunkSize = FixedStringSizeShort + FixedStringSizeLong + 32;
        internal const uint SequenceSize = FixedStringSizeShort + 52;
        internal const uint GlobalSequenceSize = 4;
        internal const uint TextureSize = FixedStringSizeLong + 8;
        internal const uint PivotPointsSize = 12;

        internal static readonly int Header = "MDLX".FromRawcode();

        internal static readonly int VersionChunk = "VERS".FromRawcode();
        internal static readonly int ModelInfoChunk = "MODL".FromRawcode();
        internal static readonly int SequencesChunk = "SEQS".FromRawcode();
        internal static readonly int GlobalSequencesChunk = "GLBS".FromRawcode();
        internal static readonly int MaterialsChunk = "MTLS".FromRawcode();
        internal static readonly int TexturesChunk = "TEXS".FromRawcode();
        internal static readonly int TextureAnimationsChunk = "TXAN".FromRawcode();
        internal static readonly int GeosetsChunk = "GEOS".FromRawcode();
        internal static readonly int GeosetAnimsChunk = "GEOA".FromRawcode();
        internal static readonly int BonesChunk = "BONE".FromRawcode();
        internal static readonly int LightsChunk = "LITE".FromRawcode();
        internal static readonly int HelpersChunk = "HELP".FromRawcode();
        internal static readonly int AttachmentsChunk = "ATCH".FromRawcode();
        internal static readonly int PivotPointsChunk = "PIVT".FromRawcode();
        internal static readonly int ParticleEmittersChunk = "PREM".FromRawcode();
        internal static readonly int ParticleEmitters2Chunk = "PRE2".FromRawcode();
        internal static readonly int RibbonEmittersChunk = "RIBB".FromRawcode();
        internal static readonly int CamerasChunk = "CAMS".FromRawcode();
        internal static readonly int EventObjectsChunk = "EVTS".FromRawcode();
        internal static readonly int CollisionShapesChunk = "CLID".FromRawcode();

        internal static readonly int EventObjectsChunk2 = "KEVT".FromRawcode(); // TODO: rename?

        internal static readonly int LayersChunk = "LAYS".FromRawcode();

        // Geosets
        internal static readonly int VerticesChunk = "VRTX".FromRawcode();
        internal static readonly int NormalsChunk = "NRMS".FromRawcode();
        internal static readonly int FaceTypeGroupsChunk = "PTYP".FromRawcode();
        internal static readonly int FaceGroupsChunk = "PCNT".FromRawcode();
        internal static readonly int FacesChunk = "PVTX".FromRawcode();
        internal static readonly int VertexGroupsChunk = "GNDX".FromRawcode();
        internal static readonly int MatrixGroupsChunk = "MTGC".FromRawcode();
        internal static readonly int MatrixIndicesChunk = "MATS".FromRawcode();
        internal static readonly int TextureCoordinateSetsChunk = "UVAS".FromRawcode();

        internal static readonly int TextureCoordinateSetChunk = "UVBS".FromRawcode(); // TODO: rename?

        // Node
        internal static readonly int NodeTranslationChunkTag = "KGTR".FromRawcode();
        internal static readonly int NodeRotationChunkTag = "KGRT".FromRawcode();
        internal static readonly int NodeScalingChunkTag = "KGSC".FromRawcode();

        // Layer
        internal static readonly int LayerTextureIdChunkTag = "KMTF".FromRawcode();
        internal static readonly int LayerAlphaChunkTag = "KMTA".FromRawcode();

        // Texture animation
        internal static readonly int TextureAnimationTranslationChunkTag = "KTAT".FromRawcode();
        internal static readonly int TextureAnimationRotationChunkTag = "KTAR".FromRawcode();
        internal static readonly int TextureAnimationScalingChunkTag = "KTAS".FromRawcode();

        // Geoset animation
        internal static readonly int GeosetAnimationAlphaChunkTag = "KGAO".FromRawcode();
        internal static readonly int GeosetAnimationColorChunkTag = "KGAC".FromRawcode();

        // Light
        internal static readonly int LightAttenuationStartChunkTag = "KLAS".FromRawcode();
        internal static readonly int LightAttenuationEndChunkTag = "KLAE".FromRawcode();
        internal static readonly int LightColorChunkTag = "KLAC".FromRawcode();
        internal static readonly int LightIntensityChunkTag = "KLAI".FromRawcode();
        internal static readonly int LightAmbientIntensityChunkTag = "KLBI".FromRawcode();
        internal static readonly int LightAmbientColorChunkTag = "KLBC".FromRawcode();
        internal static readonly int LightVisibilityChunkTag = "KLAV".FromRawcode();

        // Attachment
        internal static readonly int AttachmentVisibilityChunkTag = "KATV".FromRawcode();

        // Particle emitter
        internal static readonly int ParticleEmitterEmissionRateChunkTag = "KPEE".FromRawcode();
        internal static readonly int ParticleEmitterGravityChunkTag = "KPEG".FromRawcode();
        internal static readonly int ParticleEmitterLongitudeChunkTag = "KPLN".FromRawcode();
        internal static readonly int ParticleEmitterLatitudeChunkTag = "KPLT".FromRawcode();
        internal static readonly int ParticleEmitterLifespanChunkTag = "KPEL".FromRawcode();
        internal static readonly int ParticleEmitterSpeedChunkTag = "KPES".FromRawcode();
        internal static readonly int ParticleEmitterVisibilityChunkTag = "KPEV".FromRawcode();

        // Particle emitter 2
        internal static readonly int ParticleEmitter2EmissionRateChunkTag = "KP2E".FromRawcode();
        internal static readonly int ParticleEmitter2GravityChunkTag = "KP2G".FromRawcode();
        internal static readonly int ParticleEmitter2LatitudeChunkTag = "KP2L".FromRawcode();
        internal static readonly int ParticleEmitter2SpeedChunkTag = "KP2S".FromRawcode();
        internal static readonly int ParticleEmitter2VisibilityChunkTag = "KP2V".FromRawcode();
        internal static readonly int ParticleEmitter2VariationChunkTag = "KP2R".FromRawcode();
        internal static readonly int ParticleEmitter2LengthChunkTag = "KP2N".FromRawcode();
        internal static readonly int ParticleEmitter2WidthChunkTag = "KP2W".FromRawcode();

        // Ribbon emitter
        internal static readonly int RibbonEmitterVisibilityChunkTag = "KRVS".FromRawcode();
        internal static readonly int RibbonEmitterHeightAboveChunkTag = "KRHA".FromRawcode();
        internal static readonly int RibbonEmitterHeightBelowChunkTag = "KRHB".FromRawcode();
        internal static readonly int RibbonEmitterAlphaChunkTag = "KRAL".FromRawcode();
        internal static readonly int RibbonEmitterColorChunkTag = "KRCO".FromRawcode();
        internal static readonly int RibbonEmitterTextureSlotChunkTag = "KRTX".FromRawcode();

        // Camera
        internal static readonly int CameraTranslationChunkTag = "KCTR".FromRawcode();
        internal static readonly int CameraRotationChunkTag = "KCRL".FromRawcode();
        internal static readonly int CameraTargetTranslationChunkTag = "KTTR".FromRawcode();

#pragma warning restore SA1600
    }
}