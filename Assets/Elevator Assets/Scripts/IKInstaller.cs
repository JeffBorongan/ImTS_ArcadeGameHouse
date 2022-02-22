using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKInstaller : MonoBehaviour
{
    [SerializeField] private Transform vrHead;
    [SerializeField] private Transform vrLeftHand;
    [SerializeField] private Transform vrRightHand;

    [SerializeField] Transform[] bones = new Transform[0];

    int index = 0;

    public void InstallIK()
    {
        if (vrHead &&
            vrLeftHand &&
            vrRightHand)
        {
            #region BoneRenderer
            BoneRenderer boneRenderer = gameObject.AddComponent<BoneRenderer>();

            foreach (Transform child in transform)
            {
                if (child.name.Contains("Skeleton"))
                {
                    //Debug.Log("Found Skeleton");
                    // recursive add to bone renderer
                    Recursive(child);
                    //boneRenderer.transforms = bones;
                }
            }
            #endregion

            #region VR Constraints
            // add vr constraints and components
            GameObject vrConstraints = new GameObject("VR Constraints");

            // set parent of VR Constraints
            vrConstraints.transform.parent = gameObject.transform;
            vrConstraints.transform.position = new Vector3(0,0,0);
            #endregion

            #region RigBuilder
            Rig rig = vrConstraints.AddComponent<Rig>();
            RigBuilder rigBuilder = gameObject.AddComponent<RigBuilder>();

            List<UnityEngine.Animations.Rigging.RigLayer> layers = new List<UnityEngine.Animations.Rigging.RigLayer>(); 
            layers.Add(new UnityEngine.Animations.Rigging.RigLayer(rig));
            rigBuilder.layers = layers;
            #endregion

            #region Left and Right IK
            // add left and right IK to vr constraints
            GameObject leftArmIK = new GameObject("Left Arm IK");
            GameObject rightArmIK = new GameObject("Right Arm IK");

            leftArmIK.transform.parent = vrConstraints.transform;
            rightArmIK.transform.parent = vrConstraints.transform;

            // add target and hint to left and right IK
            // also add components
            foreach (Transform child in vrConstraints.transform)
            {
                string name = child.name;
                GameObject target = new GameObject(name + " Target");
                GameObject hint = new GameObject(name + " Hint");

                target.transform.parent = child;
                hint.transform.parent = child;

                TwoBoneIKConstraint tbIKConstraint = child.gameObject.AddComponent<TwoBoneIKConstraint>();
                tbIKConstraint.data.target = target.transform;
                tbIKConstraint.data.hint = hint.transform;

                string side;
                if (name.Contains("Right")) side = "r";
                else side = "l";

                foreach (Transform bone in bones)
                {
                    if (bone == null) break;
                    else if (bone.name == "arm_stretch_" + side) 
                        tbIKConstraint.data.root = bone;

                    else if (bone.name == "forearm_stretch_" + side)
                    {
                        tbIKConstraint.data.mid = bone;
                        hint.transform.rotation = bone.rotation;
                        hint.transform.position = bone.position - new Vector3(0, 0, 0.1f);
                    }

                    else if (bone.name == "hand_" + side)
                    {
                        tbIKConstraint.data.tip = bone;
                        target.transform.rotation = bone.rotation;
                        target.transform.position = bone.position;
                    }
                }
            }
            #endregion

            #region Head Constraint
            // add Head Constraint to VR Constraints
            GameObject headConstraint = new GameObject("Head Constraints");
            headConstraint.transform.parent = vrConstraints.transform;

            // add component
            MultiParentConstraint mpConstraint = headConstraint.AddComponent<MultiParentConstraint>();

            Transform headBone = transform;
            foreach (Transform bone in bones)
            {
                if (bone.name == "head_x")
                {
                    headBone = bone;
                    break;
                }
            }

            headConstraint.transform.position = headBone.position;
            headConstraint.transform.rotation = headBone.rotation;
            mpConstraint.data.constrainedObject = headBone;

            WeightedTransformArray wTransformArray = new WeightedTransformArray(1);
            wTransformArray.Clear();
            wTransformArray.Add(new WeightedTransform(headConstraint.transform, 1));
            mpConstraint.data.sourceObjects = wTransformArray;
            #endregion

            #region VR Rig
            VRRig vrRig = gameObject.AddComponent<VRRig>();

            //vrRig.turnSmoothness = 5;
            vrRig.headConstaint = headConstraint.transform;

            vrRig.head.vrTarget = vrHead;
            vrRig.head.rigTarget = headConstraint.transform;
            vrRig.head.trackingPositionOffset = new Vector3(0f, -0.1f, -0.3f);
            vrRig.head.trackingRotationOffset = new Vector3(20, 0, 0);

            vrRig.leftHand.vrTarget = vrLeftHand;
            vrRig.leftHand.rigTarget = leftArmIK.GetComponent<TwoBoneIKConstraint>().data.target;
            vrRig.leftHand.trackingRotationOffset = new Vector3(0, -90, -90);

            vrRig.rightHand.vrTarget = vrRightHand;
            vrRig.rightHand.rigTarget = rightArmIK.GetComponent<TwoBoneIKConstraint>().data.target;
            vrRig.rightHand.trackingRotationOffset = new Vector3(0, 90, 90);
            #endregion

            DestroyImmediate(this);
        }
        else
        {
            Debug.LogError("Variables needed to be initialized to continue");
        }
    }

    void Recursive(Transform parent)
    {
        if (parent.childCount > 0)
            foreach (Transform child in parent)
            {
                ArrayAdd(bones, child);
                Recursive(child);
            }
    }

    void ArrayAdd(Transform[] array, Transform value)
    {
        array[index] = value;
        index++;
    }
}
