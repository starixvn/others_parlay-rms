export class Sort {
    private sortOrder = 1;
    
    constructor(){}

    public startSort(property: string, order: string)
    {
        if (order === "desc")
        {
            this.sortOrder = -1;
        }
        return (a: any, b: any) => (a[property] - b[property]) * this.sortOrder;
    }
}