using System.Collections.Generic;
using Godot;

namespace DeadDog.RecallPast.Libs.Nodeo.Input;

public static class GodotInputMapController
{
    #region 辅助字段
    /// <summary>
    /// 动作白名单
    /// 该名单下的动作不会被删除
    /// </summary>
    private static readonly HashSet<string> _whitelist =
    [
        "ui_accept",
        "ui_select",
        "ui_cancel",
        "ui_focus_next",
        "ui_focus_prev",
        "ui_left",
        "ui_right",
        "ui_up",
        "ui_down",
        "ui_page_up",
        "ui_page_down",
        "ui_home",
        "ui_end",
        "ui_accessibility_drag_and_drop",
        "ui_cut",
        "ui_copy",
        "ui_focus_mode",
        "ui_paste",
        "ui_undo",
        "ui_redo",
        "ui_text_completion_query",
        "ui_text_completion_accept",
        "ui_text_completion_replace",
        "ui_text_newline",
        "ui_text_newline_blank",
        "ui_text_newline_above",
        "ui_text_indent",
        "ui_text_dedent",
        "ui_text_backspace",
        "ui_text_backspace_word",
        "ui_text_backspace_word.macos",
        "ui_text_backspace_all_to_left",
        "ui_text_backspace_all_to_left.macos",
        "ui_text_delete",
        "ui_text_delete_word",
        "ui_text_delete_word.macos",
        "ui_text_delete_all_to_right",
        "ui_text_delete_all_to_right.macos",
        "ui_text_caret_left",
        "ui_text_caret_word_left",
        "ui_text_caret_word_left.macos",
        "ui_text_caret_right",
        "ui_text_caret_word_right",
        "ui_text_caret_word_right.macos",
        "ui_text_caret_up",
        "ui_text_caret_down",
        "ui_text_caret_line_start",
        "ui_text_caret_line_start.macos",
        "ui_text_caret_line_end",
        "ui_text_caret_line_end.macos",
        "ui_text_caret_page_up",
        "ui_text_caret_page_down",
        "ui_text_caret_document_start",
        "ui_text_caret_document_start.macos",
        "ui_text_caret_document_end",
        "ui_text_caret_document_end.macos",
        "ui_text_caret_add_below",
        "ui_text_caret_add_below.macos",
        "ui_text_caret_add_above",
        "ui_text_caret_add_above.macos",
        "ui_text_scroll_up",
        "ui_text_scroll_up.macos",
        "ui_text_scroll_down",
        "ui_text_scroll_down.macos",
        "ui_text_select_all",
        "ui_text_select_word_under_caret",
        "ui_text_select_word_under_caret.macos",
        "ui_text_add_selection_for_next_occurrence",
        "ui_text_skip_selection_for_next_occurrence",
        "ui_text_clear_carets_and_selection",
        "ui_text_toggle_insert_mode",
        "ui_menu",
        "ui_text_submit",
        "ui_unicode_start",
        "ui_graph_duplicate",
        "ui_graph_delete",
        "ui_graph_follow_left",
        "ui_graph_follow_left.macos",
        "ui_graph_follow_right",
        "ui_graph_follow_right.macos",
        "ui_filedialog_up_one_level",
        "ui_filedialog_refresh",
        "ui_filedialog_show_hidden",
        "ui_swap_input_direction",
        "ui_colorpicker_delete_preset",
    ];
    #endregion
    #region 操作
    /// <summary>
    /// 覆盖Godot的自定义输入映射
    /// </summary>
    /// <param name="dic">新映射</param>
    public static void OverrideGodotCustomInputMap(Dictionary<string, InputEvent[]> dic)
    {
        ClearGodotCustomInputMap();
        foreach (var (action, events) in dic)
        {
            InputMap.AddAction(action);
            foreach (var @event in events)
            {
                InputMap.ActionAddEvent(action,@event);
            }
        }
    }

    /// <summary>
    /// 覆盖Godot的自定义输入映射
    /// </summary>
    /// <param name="mapDto">新映射数据</param>   
    public static void OverrideGodotCustomInputMap(NodeoInputMapDto mapDto)
    {
        var dic = NodeoInputParser.ParseMapDtoToDic(mapDto);
        OverrideGodotCustomInputMap(dic);
    }

    /// <summary>
    /// 清空Godot的自定义输入映射
    /// </summary>
    public static void ClearGodotCustomInputMap()
    {
        var actions = InputMap.GetActions();
        var length = actions.Count;
        for (int i = 0; i < length; i++) 
            if (!_whitelist.Contains(actions[i]))
                InputMap.EraseAction(actions[i]);
    }
    #endregion
}