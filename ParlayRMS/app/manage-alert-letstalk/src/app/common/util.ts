export class Sort {
    private sortOrder = 1;
    
    constructor(){}

    public startSort(property: string, order: string, type: string)
    {
        if (order === "desc") {
            this.sortOrder = -1;
        }
        switch(type) {
            case "number": {
                return (a: any, b: any) => (a[property] - b[property]) * this.sortOrder;
            }
            case "string": {
                return (a: any, b: any) => {
                    if (a[property] === null) {
                        return 1 * this.sortOrder;
                    }
                    if (b[property] === null) {
                        return -1 * this.sortOrder;
                    }
                    return (a[property].trim().localeCompare(b[property].trim())) * this.sortOrder;
                };
            }
            default:
                return 0;
        }
        
    }
}