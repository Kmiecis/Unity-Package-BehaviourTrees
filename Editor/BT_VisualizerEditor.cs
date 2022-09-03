using Common.BehaviourTrees;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.BehaviourTrees
{
    [CustomEditor(typeof(BT_Visualizer))]
    public class BT_VisualizerEditor : Editor
    {
        private GUIStyle _labelStyle;
        private GUIStyle _foldoutStyle;
        private Dictionary<BT_ITask, bool> _foldouts = new Dictionary<BT_ITask, bool>();

        private BT_Visualizer Script
        {
            get => (BT_Visualizer)target;
        }
        
        private GUIStyle LabelStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle(EditorStyles.label);
                    _labelStyle.richText = true;
                    _labelStyle.normal = new GUIStyleState { textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black };
                    _labelStyle.padding = new RectOffset(-2, 0, 0, 0);
                }
                return _labelStyle;
            }
        }

        private GUIStyle FoldoutStyle
        {
            get
            {
                if (_foldoutStyle == null)
                {
                    _foldoutStyle = new GUIStyle(EditorStyles.foldout);
                    _foldoutStyle.richText = true;
                }
                return _foldoutStyle;
            }
        }

        public override void OnInspectorGUI()
        {
            var tasks = Script.Tasks;

            if (tasks != null && tasks.Length != 0)
            {
                EditorGUILayout.BeginVertical();

                const int BASE_INDENT = 0;
                const int BASE_LINE = 1;

                AddTask(tasks[0], BASE_INDENT, BASE_LINE);
                for (int i = 1; i < tasks.Length; ++i)
                {
                    EditorGUILayout.Space();
                    AddTask(tasks[i], BASE_INDENT, BASE_LINE);
                }

                EditorGUILayout.EndVertical();
            }

            EditorUtility.SetDirty(target);
        }

        private void AddTask(BT_ITask itask, int indent, int line)
        {
            if (indent != 0 && Script.Tasks.Contains(itask))
            {
                AddLabelField(itask, indent, line);
            }
            else if (itask is BT_ASingleNode single)
            {
                TryAddConditionalsField(single, indent, ref line);
                var foldout = AddFoldoutField(single, indent, line++);
                TryAddDecoratorsField(single, indent, ref line);
                if (foldout)
                {
                    var task = single.Task;
                    if (task != null)
                    {
                        AddTask(task, indent + 1, line);
                    }
                }
            }
            else if (itask is BT_AMultiNode multi)
            {
                TryAddConditionalsField(multi, indent, ref line);
                var foldout = AddFoldoutField(multi, indent, line++);
                TryAddDecoratorsField(multi, indent, ref line);
                if (foldout)
                {
                    var tasks = multi.Tasks;
                    if (tasks != null)
                    {
                        for (int i = 0; i < tasks.Length; ++i)
                        {
                            AddTask(tasks[i], indent + 1, line++);
                        }
                    }
                }
            }
            else if (itask is BT_ATask task)
            {
                TryAddConditionalsField(task, indent, ref line);
                AddLabelField(task, indent, line++);
                TryAddDecoratorsField(task, indent, ref line);
            }
            else
            {
                AddLabelField(itask, indent, line);
            }
        }

        private void TryAddServicesField(BT_ATask task, int indent, ref int line)
        {
            var services = task.Services;
            if (!services.IsNullOrEmpty())
            {
                EditorGUILayout.LabelField(GetLine(line++) + GetIndent(indent) + GetServices(services), LabelStyle);
            }
        }

        private void TryAddConditionalsField(BT_ATask task, int indent, ref int line)
        {
            var conditionals = task.Conditionals;
            if (!conditionals.IsNullOrEmpty())
            {
                EditorGUILayout.LabelField(GetLine(line++) + GetIndent(indent) + GetConditionals(conditionals), LabelStyle);
            }
        }

        private void TryAddDecoratorsField(BT_ATask task, int indent, ref int line)
        {
            var decorators = task.Decorators;
            if (!decorators.IsNullOrEmpty())
            {
                EditorGUILayout.LabelField(GetLine(line++) + GetIndent(indent) + GetDecorators(decorators), LabelStyle);
            }
        }

        private bool AddFoldoutField(BT_ITask task, int indent, int line)
        {
            var foldout = _foldouts.GetOrCompute(task, delegate { return true; });
            foldout = EditorGUILayout.Foldout(foldout, GetLine(line) + GetIndent(indent) + GetTaskTitle(task), true, FoldoutStyle);
            _foldouts[task] = foldout;
            return foldout;
        }

        private void AddLabelField(BT_ITask task, int indent, int line)
        {
            EditorGUILayout.LabelField(GetLine(line) + GetIndent(indent) + GetTaskTitle(task), LabelStyle);
        }
        
        private string GetTaskColor(BT_EStatus status)
        {
            switch (status)
            {
                case BT_EStatus.Failure:
                    return "FF0000";

                case BT_EStatus.Success:
                    return "00FF00";

                case BT_EStatus.Running:
                    return "FFFF00";
            }
            return "000000";
        }

        private string GetTaskPrefix(BT_ITask task)
        {
            if (task is BT_TreeNode)
            {
                return "Tree";
            }
            else if (task is BT_SequenceNode)
            {
                return "Sequence";
            }
            else if (task is BT_SelectorNode)
            {
                return "Selector";
            }
            else if (task is BT_ParallelNode)
            {
                return "Parallel";
            }
            else if (task is BT_RaceNode)
            {
                return "Race";
            }
            else if (task is BT_RandomNode)
            {
                return "Random";
            }
            return null;
        }

        private string GetTaskString(BT_ITask task)
        {
            var taskName = task.ToString();
            var taskPrefix = GetTaskPrefix(task);
            if (taskPrefix != null)
            {
                if (taskName.IsEmpty())
                {
                    return taskPrefix;
                }
                else
                {
                    return taskPrefix + " \'" + taskName + '\'';
                }
            }
            else
            {
                return taskName;
            }
        }

        private string GetTaskTitle(BT_ITask task)
        {
            return "<color=#" + GetTaskColor(task.Status) + '>' + GetTaskString(task) + "</color>";
        }

        private string GetServices(BT_IService[] services)
        {
            return "Serve " + GetItems(services);
        }

        private string GetConditionals(BT_IConditional[] conditionals)
        {
            return "While " + GetItems(conditionals);
        }

        private string GetDecorators(BT_IDecorator[] decorators)
        {
            return "With " + GetItems(decorators);
        }

        private string GetItems(object[] objects)
        {
            var values = new string[objects.Length];

            values[0] = '\'' + objects[0].ToString() + "\' ";
            for (int i = 1; i < objects.Length; ++i)
            {
                values[i] = "& \'" + objects[i].ToString() + "\' ";
            }

            return string.Concat(values);
        }

        private string GetLine(int line)
        {
            return line.ToString() + ".\t";
        }

        private string GetIndent(int indent)
        {
            var values = new string[indent];
            for (int i = 0; i < indent; ++i)
            {
                values[i] = "    ";
            }
            return string.Concat(values);
        }
    }
}
