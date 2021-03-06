import * as React from 'react';
import IconButton from "material-ui/IconButton";
import Toolbar from "material-ui/Toolbar";
import ToolbarGroup from "material-ui/Toolbar/ToolbarGroup";
import TextField from "material-ui/TextField";
import List from "material-ui/List";
import Subheader from "material-ui/Subheader";
import Divider from "material-ui/Divider";

import { FilteredListItem } from "./FilteredList";
import { join } from '../fragments/partial/util';
export { FilteredListItem } from "./FilteredList";

export interface FilteredListItemGroup {
    /** The heading content */
    heading: string;
    items: FilteredListItem[];
}

export interface FilteredGroupedListProps {
    /** The current filter, treated as a case-insensitive regular expression */
    filter?: string;
    filterChanged: (filter: string | undefined) => void;
    groups: FilteredListItemGroup[];
}

function generateRegex(filter: string | undefined) {
    try { return new RegExp(filter || "", "i"); }
    catch { return new RegExp("", "i"); }
}

function headingList(group: FilteredListItemGroup) {
    if (group.items.length === 0)
        return null;
    return [
        <List key={group.heading}>
            <Subheader>{group.heading}</Subheader>
            {group.items.map(x => x.content)}
        </List>
    ];
}

export const FilteredGroupedList: React.StatelessComponent<FilteredGroupedListProps> = ({ filter, filterChanged, groups }) => {
    if (filter === "")
        filter = undefined;
    const regex = generateRegex(filter);
    const filteredGroups = groups.map(x => ({ heading: x.heading, items: x.items.filter(item => regex.test(item.search)) }));
    const fullCount = groups.reduce((sum, value) => sum + value.items.length, 0);
    const filteredCount = filteredGroups.reduce((sum, value) => sum + value.items.length, 0);
    const filteredMessage = !filter ? null : <span>{filteredCount} of {fullCount}</span>;
    const clearFilterButton = !filter ? null : <IconButton tooltip="Clear filter" onClick={() => filterChanged(undefined)} iconClassName="material-icons">clear</IconButton>;

    return (
        <div>
            <Toolbar>
                <ToolbarGroup style={{flex: "auto"}}>
                    <TextField hintText="Filter" value={filter || ""} onChange={(e, newValue) => filterChanged(newValue ? newValue : undefined)} style={{flex: "auto"}}/>
                </ToolbarGroup>
                <ToolbarGroup lastChild={true} style={{marginLeft: "24px"}}>
                    {filteredMessage}
                    {clearFilterButton}
                </ToolbarGroup>
            </Toolbar>
            {join(filteredGroups.map(x => headingList(x)).filter(x => x !== null), [<Divider/>])}
        </div>
    );
};