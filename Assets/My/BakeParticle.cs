using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

[ExecuteInEditMode]
public class BakeParticle : MonoBehaviour
{
	private static readonly int mainTexID = Shader.PropertyToID("_MainTex");
	private static readonly List<Vector3> vertices = new List<Vector3>();

	public Action<BakeParticle> updateEvent;

	public int bakeTime = 2; //提高效率 默认为2不建议修改

	public ParticleSystem particleSystem;
	public ParticleSystemRenderer particleSystemRenderer;

	private int updateTimer;
	private Camera uiCam;
	private Mesh uiMesh;
	private Texture mainTexture;
	private bool isTrail;
	private BakeParticle trailParticle;

	public Mesh UIMesh => uiMesh;

	//FGUI 的 UI Shader 要专门写
	public Material MainMaterial => particleSystemRenderer ? particleSystemRenderer.sharedMaterial : null;

	public Texture MainTexture => mainTexture ? mainTexture : Texture2D.whiteTexture;


	private void OnEnable()
	{
		if (particleSystem == null)
		{
			particleSystem = GetComponent<ParticleSystem>();
		}

		if (particleSystemRenderer == null)
		{
			if (particleSystem != null)
			{
				particleSystemRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
			}
		}

		uiCam = StageCamera.main ? StageCamera.main : Camera.main; //这里需要FGUI camera

		updateTimer = 0;
		uiMesh = new Mesh {name = "UI Mesh"};
		uiMesh.MarkDynamic();
		CheckTrail();
	}

	private void OnDisable()
	{
		if (uiMesh)
		{
#if UNITY_EDITOR
			DestroyImmediate(uiMesh);
#else
			Destroy(uiMesh); //注意FGUI 可能还有自己的卸载
#endif
			uiMesh = null;
		}

		CheckTrail();
	}

	private void Update()
	{
		if (updateTimer <= 0)
		{
			updateTimer = bakeTime;
			UpdateMesh();
			UpdateMainTexture();
			if (updateEvent != null)
			{
				updateEvent(this);
			}
		}
		else
		{
			--updateTimer;
		}
	}

	private void UpdateMesh()
	{
		if (particleSystem == null || particleSystemRenderer == null)
		{
			return;
		}

#if UNITY_EDITOR
		if (Application.isPlaying == false)
		{
			return;
		}
#endif

		particleSystemRenderer.enabled = false;
		bool useTransform = false;
		Matrix4x4 matrix = Matrix4x4.identity;

		switch (particleSystem.main.simulationSpace)
		{
			case ParticleSystemSimulationSpace.Local:
			{
				matrix = Matrix4x4.Rotate(particleSystem.transform.rotation).inverse
				         * Matrix4x4.Scale(particleSystem.transform.lossyScale).inverse;
				useTransform = true;
				break;
			}
			case ParticleSystemSimulationSpace.World:
			{
				matrix = particleSystem.transform.worldToLocalMatrix;
				break;
			}
			case ParticleSystemSimulationSpace.Custom:
			{
				break;
			}
		}

		uiMesh.Clear();
		if (particleSystem.particleCount > 0)
		{
			if (isTrail)
			{
				particleSystemRenderer.BakeTrailsMesh(uiMesh, uiCam, useTransform);
			}
			else
			{
				particleSystemRenderer.BakeMesh(uiMesh, uiCam, useTransform);
			}

			uiMesh.GetVertices(vertices);
			var count = vertices.Count;
			for (int i = 0; i < count; i++)
			{
				vertices[i] = matrix.MultiplyPoint3x4(vertices[i]);
			}

			uiMesh.SetVertices(vertices);
			vertices.Clear();
		}
	}

	private void UpdateMainTexture()
	{
		Texture tempTexture = null;

		if (isTrail == false)
		{
			var textureSheet = particleSystem.textureSheetAnimation;
			if (textureSheet.enabled && textureSheet.mode == ParticleSystemAnimationMode.Sprites
			                         && textureSheet.spriteCount > 0)
			{
				tempTexture = textureSheet.GetSprite(0).texture;
			}
		}

		if (tempTexture == null && particleSystemRenderer)
		{
			Material mat;

			if (isTrail)
			{
				mat = particleSystemRenderer.trailMaterial;
			}
			else
			{
				// if (Application.isPlaying)
				// {
				// 	mat = particleSystemRenderer.material;
				// }
				// else
				// {
				mat = particleSystemRenderer.sharedMaterial;
				// }
			}

			if (mat && mat.HasProperty(mainTexID))
			{
				tempTexture = mat.mainTexture;
			}
		}

		mainTexture = tempTexture ? tempTexture : Texture2D.whiteTexture;
	}

	private void CheckTrail()
	{
		if (isActiveAndEnabled && !isTrail && particleSystem && particleSystem.trails.enabled)
		{
			if (trailParticle == null)
			{
				trailParticle = new GameObject("[UIParticle] Trail").AddComponent<BakeParticle>();
				var ts = trailParticle.transform;
				ts.SetParent(transform);
				ts.localPosition = Vector3.zero;
				ts.localRotation = Quaternion.identity;
				ts.localScale = Vector3.one;

				trailParticle.particleSystem = this.particleSystem;
				trailParticle.particleSystemRenderer = this.particleSystemRenderer;
				trailParticle.isTrail = true;
			}

			trailParticle.enabled = true;
		}
		else if (trailParticle)
		{
			trailParticle.enabled = false;
		}
	}
}