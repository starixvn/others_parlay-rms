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
                    if (a[property] === null && b[property] === null) {
                        return 0;
                    }
                    if (a[property] === null) {
                        return 1 * this.sortOrder;
                    }
                    if (b[property] === null) {
                        return -1 * this.sortOrder;
                    }
                    return (a[property].toString().trim().localeCompare(b[property].toString().trim())) * this.sortOrder;
                };
            }
            case "datetime": {
                return (a: any, b: any) => {
                    const dateA = new Date(a[property]);
                    const dateB = new Date(b[property]);
                    return (dateA.getTime() - dateB.getTime()) * this.sortOrder;
                };
            }
            default:
                return 0;
        }
        
    }
}