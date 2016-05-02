using UnityEngine;
using UnityEditor;

public class ImportModelAssetFix:AssetPostprocessor
{
	private bool _useThis = false;
	
	void OnPreprocessModel()
	{
		if (!_useThis) return;
		ModelImporter modelImporter = (ModelImporter)assetImporter;
		// meshes
		modelImporter.globalScale = 1;
		modelImporter.isReadable = false;
		modelImporter.addCollider = false;
		modelImporter.importAnimation = false;
		modelImporter.optimizeMesh = true;
		modelImporter.meshCompression = ModelImporterMeshCompression.Off;
		modelImporter.swapUVChannels = false;
		modelImporter.generateSecondaryUV = false;
		// normals & tangents
		modelImporter.normalImportMode = ModelImporterTangentSpaceMode.Calculate;
		modelImporter.tangentImportMode = ModelImporterTangentSpaceMode.Calculate;
		// materials
		modelImporter.importMaterials = false;
	}
	
	void OnPreprocessTexture()
	{
		if (!_useThis) return;
		TextureImporter textureImporter = (TextureImporter)assetImporter;
		textureImporter.textureType = TextureImporterType.Advanced;
		textureImporter.mipmapEnabled = false;
		textureImporter.compressionQuality = (int)TextureCompressionQuality.Best;
		textureImporter.textureFormat = TextureImporterFormat.ETC_RGB4;
		// filter wrap mode
		textureImporter.filterMode = FilterMode.Bilinear;
		textureImporter.anisoLevel = 0;
		textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
	}
	
	void OnPreprocessAudio()
	{
		if (!_useThis) return;
		AudioImporter audioImporter = (AudioImporter)assetImporter;
		audioImporter.forceToMono = true;
		audioImporter.compressionBitrate = 128;
		audioImporter.format = AudioImporterFormat.Compressed;
		audioImporter.threeD = false;
		audioImporter.loopable = false;
		audioImporter.loadType = AudioImporterLoadType.CompressedInMemory;
	}
}